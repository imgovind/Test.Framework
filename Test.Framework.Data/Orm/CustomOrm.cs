using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Extensions;

namespace Test.Framework.Data
{
    public class CustomOrm : Disposable, IOrm
    {
        #region Private Members

        private IDbConnection _dbConnection;

        #endregion

        #region Constructor

        public CustomOrm(IDbConnection connection)
        {
            _dbConnection = connection;
        }

        public void OpenConnection()
        {
            if (_dbConnection.State != ConnectionState.Open) _dbConnection.Open();
        }

        public IDbConnection GetConnection()
        {
            OpenConnection();
            return _dbConnection;
        } 

        #endregion

        #region IDataAccessConnection Members

        public bool Execute(SqlDbCommand query)
        {
            OpenConnection();
            return _dbConnection.CreateCustomCommand(query).ExecuteNonQuery() > 0 ? true : false;
        }
        public async Task<bool> ExecuteAsync(SqlDbCommand query)
        {
            OpenConnection();
            var result = await Task.Run<bool>(() => Execute(query));
            return result;
        }

        public bool Execute(IList<SqlDbCommand> queries)
        {
            if (queries == null || !queries.Any()) return false;
            OpenConnection();
            var errorMessage = string.Empty;
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (queries.Count == 1 && queries.First() != null)
                        _dbConnection.CreateCustomCommand(queries.First()).ExecuteNonQuery();

                    queries.ForEach(query => {
                        _dbConnection.CreateCustomCommand(query).ExecuteNonQuery();
                    });
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    //Log the Exception
                    return false;
                }
            }
        }

        public async Task<bool> ExecuteAsync(IList<SqlDbCommand> queries)
        {
            var result = await Task.Run(() => Execute(queries));
            return result;
        }

        public IEnumerable<T> Select<T>(SqlDbCommand query, ISelectable<T> traits = null) 
            where T : class, new()
        {
            OpenConnection();
            using (IDataReader dataReader = _dbConnection.CreateCustomCommand(query).ExecuteReader(CommandBehavior.CloseConnection))
            {
                return dataReader.Hydrate<T>(traits);
            }
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(SqlDbCommand query, ISelectable<T> traits = null)
            where T : class, new()
        {
            var result = await Task.Run<IEnumerable<T>>(() => Select<T>(query, traits));
            return result;
        }

        public IEnumerable<T> Select<T>(SqlDbCommand query, Func<DataReader, T> readMapper) 
            where T : class, new()
        {
            OpenConnection();
            using (IDataReader dataReader = _dbConnection.CreateCustomCommand(query).ExecuteReader(CommandBehavior.CloseConnection))
            {
                return dataReader.Hydrate<T>(readMapper);
            }
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(SqlDbCommand query, Func<DataReader, T> readMapper)
            where T : class, new()
        {
            var result = await Task.Run<IEnumerable<T>>(() => Select<T>(query, readMapper));
            return result;
        }

        #endregion

        #region Private Methods
        private int GetLastInsertRowId()
        {
            using (IDbCommand command = _dbConnection.CreateCommand())
            {
                command.CommandText = GetLastInsertedAutoNumberStatement();
                using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    return reader.Read() ? Int32.Parse(reader[0].ToString()) : 0;
                }
            }
        }

        private string GetLastInsertedAutoNumberStatement()
        {
            switch (_dbConnection.GetType().Name.ToUpper().Replace("CONNECTION", ""))
            {
                case "SQLITE": return "select last_insert_rowid()";
                case "SQL": return "select @@identity";
                case "NPGSQL": return "select lastval()";
                case "MYSQL": return "SELECT LAST_INSERT_ID();";
            }
            throw new ApplicationException("Unknown autonumber statement");
        } 
        #endregion

        #region Disposable Overrides

        protected override void Dispose(bool disposing)
        {
            if (disposing && this._dbConnection != null)
            {
                if (this._dbConnection.State != ConnectionState.Closed)
                {
                    this._dbConnection.Close();
                    this._dbConnection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion  Disposable Overrides
    }
}
