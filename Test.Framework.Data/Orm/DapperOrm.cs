using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Test.Framework.Extensions;
using System.Data.SqlClient;

namespace Test.Framework.Data
{
    public class DapperOrm : Disposable, IOrm
    {
        #region Private Members

        private string connectionName;
        private SqlDbmsType sqlDbType;

        #endregion

        #region Constructor

        public DapperOrm(string connectionName, SqlDbmsType sqlDbType)
        {
            this.connectionName = connectionName;
            this.sqlDbType = sqlDbType;
        }

        public IDbConnection GetConnection()
        {
            return new Connection(this.connectionName).Get();
        }

        #endregion

        #region IDataAccessConnection Members

        public bool Execute(SqlDbCommand query)
        {
            using (IDbConnection dbConnection = new Connection(this.connectionName).Get())
            {
                var result = dbConnection.Execute(dbConnection.CreateDapperCommand(query)) > 0 ? true : false;
                return result;
            }
        }

        public async Task<bool> ExecuteAsync(SqlDbCommand query)
        {
            using (IDbConnection dbConnection = new Connection(this.connectionName).Get())
            {
                var result = await dbConnection.ExecuteAsync(dbConnection.CreateDapperCommand(query)) > 0;
                return result;
            }
        }

        public bool Execute(IList<SqlDbCommand> queries)
        {
            using (IDbConnection dbConnection = new Connection(this.connectionName).Get())
            {
                if (queries == null || !queries.Any()) return false;
                var errorMessage = string.Empty;
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        if (queries.Count == 1 && queries.First() != null)
                        {
                            var command = dbConnection.CreateDapperCommand(queries.First(), transaction);
                            dbConnection.Execute(command);
                        }
                        queries.ForEach(query =>
                        {
                            errorMessage = query.ErrorMessage;
                            dbConnection.Execute(dbConnection.CreateDapperCommand(query, transaction));
                        });
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        //Log this exception
                        return false;
                    }
                }
            }
        }

        public async Task<bool> ExecuteAsync(IList<SqlDbCommand> queries)
        {
            using (IDbConnection dbConnection = new Connection(this.connectionName).Get())
            {
                var result = false;

                if (queries == null || !queries.Any())
                    return result;

                var errorMessage = string.Empty;
                List<Task<int>> transactionalQueries = new List<Task<int>>();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        if (queries.Count == 1 && queries.First() != null)
                        {
                            result = await dbConnection.ExecuteAsync(dbConnection.CreateDapperCommandAsync(queries.First(), transaction)) > 0;
                        }
                        else
                        {
                            queries.ForEach(query =>
                            {
                                errorMessage = query.ErrorMessage;
                                transactionalQueries.Add(dbConnection.ExecuteAsync(dbConnection.CreateDapperCommandAsync(query, transaction)));
                            });

                            var results = await Task.WhenAll(transactionalQueries);
                            result = true;
                        }
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        //Log this exception
                        return result;
                    }
                }
            }
        }

        public IEnumerable<T> Select<T>(SqlDbCommand query, ISelectable<T> traits = null)
            where T : class, new()
        {
            IEnumerable<T> result = null;
            using (IDbConnection dbConnection = new Connection(this.connectionName).Get())
            {
                switch (this.sqlDbType)
                {
                    case SqlDbmsType.MySql:
                        if (traits == null)
                            result = dbConnection.Query<T>(dbConnection.CreateDapperCommand(query));
                        else
                            result = SelectWithTraits<T>(query, traits);
                        break;
                    case SqlDbmsType.SqlServer:
                        if(traits == null)
                            result = dbConnection.Query<T>(dbConnection.CreateDapperCommand(query)).ToList();
                        else 
                            result = SelectWithTraits<T>(query, traits);
                        break;
                    case SqlDbmsType.Oracle:
                    case SqlDbmsType.SqlLite:
                    case SqlDbmsType.PostGreSql:
                    default:
                        break;
                }
            }
            return result;
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(SqlDbCommand query, ISelectable<T> traits = null)
            where T : class, new()
        {
            IEnumerable<T> result = null;
            using (IDbConnection dbConnection = new Connection(this.connectionName).Get())
            {
                switch (this.sqlDbType)
                {
                    case SqlDbmsType.MySql:
                        if (traits == null)
                            result = await dbConnection.QueryAsync<T>(dbConnection.CreateDapperCommandAsync(query));
                        else
                            result = await SelectWithTraitsAsync<T>(query, traits);
                        break;
                    case SqlDbmsType.SqlServer:
                        if (traits == null)
                        {
                            var tempResult = await dbConnection.QueryAsync<T>(dbConnection.CreateDapperCommandAsync(query));
                            result = tempResult.ToList();
                        }
                        else
                        {
                            var tempResult = await SelectWithTraitsAsync<T>(query, traits);
                            result = tempResult.ToList();
                        }
                        break;
                    case SqlDbmsType.Oracle:
                    case SqlDbmsType.SqlLite:
                    case SqlDbmsType.PostGreSql:
                    default:
                        break;
                }
            }
            return result;
        }

        public IEnumerable<T> Select<T>(SqlDbCommand query, Func<DataReader, T> readMapper)
            where T : class, new()
        {
            IEnumerable<T> result = null;
            using (IDbConnection dbConnection = new Connection(this.connectionName).Get())
            {
                using (IDataReader dataReader = dbConnection.CreateCustomCommand(query).ExecuteReader(CommandBehavior.CloseConnection))
                {
                    switch (this.sqlDbType)
                    {
                        case SqlDbmsType.MySql:
                            result = dataReader.Hydrate<T>(readMapper);
                            break;
                        case SqlDbmsType.SqlServer:
                            var tempResult = dataReader.Hydrate<T>(readMapper);
                            result = tempResult.ToList();
                            break;
                        case SqlDbmsType.Oracle:
                        case SqlDbmsType.SqlLite:
                        case SqlDbmsType.PostGreSql:
                        default:
                            break;
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(SqlDbCommand query, Func<DataReader, T> readMapper)
            where T : class, new()
        {
            IEnumerable<T> result = null;
            using (IDbConnection dbConnection = new Connection(this.connectionName).Get())
            {
                switch (this.sqlDbType)
                {
                    case SqlDbmsType.MySql:
                        result = await Task.Run<IEnumerable<T>>(() => Select<T>(query, readMapper));
                        break;
                    case SqlDbmsType.SqlServer:
                        var tempResult = await Task.Run<IEnumerable<T>>(() => Select<T>(query, readMapper));
                        result = tempResult.ToList();
                        break;
                    case SqlDbmsType.Oracle:
                    case SqlDbmsType.SqlLite:
                    case SqlDbmsType.PostGreSql:
                    default:
                        break;
                }
            }
            return result;
        }

        #endregion

        #region Private Methods

        private IEnumerable<T> SelectWithTraits<T>(SqlDbCommand query, ISelectable<T> traits)
            where T : class, new()
        {
            using (IDbConnection dbConnection = new Connection(this.connectionName).Get())
            {
                using (IDataReader dataReader = dbConnection.CreateCustomCommand(query).ExecuteReader(CommandBehavior.CloseConnection))
                {
                    var result = dataReader.Hydrate<T>(traits);
                    return result;
                }
            }
        }

        private async Task<IEnumerable<T>> SelectWithTraitsAsync<T>(SqlDbCommand query, ISelectable<T> traits)
            where T : class, new()
        {
            var result = await Task.Run<IEnumerable<T>>(() => SelectWithTraits<T>(query, traits));
            return result;
        }

        #endregion
    }
}
