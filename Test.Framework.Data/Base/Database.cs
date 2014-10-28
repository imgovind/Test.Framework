using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Test.Framework.Extensions;

namespace Test.Framework.Data
{
    public class Database : IDatabase
    {
        public string ConnectionString { get; private set; }

        public IDbConnection Connection { get { return orm.GetConnection(); } }
        public IDbConnection dbConnection { get; set; }
        public IOrm orm { get; set; }

        public Database(IDbConnection dbConnection, IOrm orm)
        {
            Ensure.Argument.IsNotNull(dbConnection, "dbConnection");
            this.dbConnection = dbConnection;
            Ensure.Argument.IsNotNull(orm, "dataAccessConnection");
            this.orm = orm;
        }

        #region Execute

        public bool Execute<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            using (IOrm orm = this.orm)
            {
                return orm.Execute(new SqlCommand(sql, parameters, timeout));
            }
        }

        public async Task<bool> ExecuteAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            var result = false;
            using (IOrm orm = this.orm)
            {
                result = await orm.ExecuteAsync(new SqlCommand(sql, parameters, timeout));
            }
            return result;
        }

        #endregion

        #region Transaction

        public bool Execute(IList<SqlCommand> unitOfWorks)
        {
            using (IOrm connection = this.orm)
            {
                return connection.Execute(unitOfWorks);
            }
        }

        public async Task<bool> ExecuteAsync(IList<SqlCommand> unitOfWorks)
        {
            var result = false;
            using (IOrm orm = this.orm)
            {
                result = await orm.ExecuteAsync(unitOfWorks);
            }
            return result;
        }

        #endregion

        #region Create

        public bool Insert<T>(T entity, int timeout) where T : class, new()
        {
            if (entity != null)
            {
                SqlCommand queryResult = DynamicQuery.GetInsertQuery(typeof(T).Name.ToLowerInvariant().Pluralize(), entity);
                using (IOrm orm = this.orm)
                {
                    return orm.Execute(new SqlCommand(queryResult.Statement, queryResult.Parameters, timeout));
                }
            }
            return false;
        }
        public async Task<bool> InsertAsync<T>(T entity, int timeout = 15) where T : class, new()
        {
            var result = false;
            if (entity != null)
            {
                SqlCommand queryResult = DynamicQuery.GetInsertQuery(typeof(T).Name.ToLowerInvariant().Pluralize(), entity);
                using (IOrm orm = this.orm)
                {
                    result = await orm.ExecuteAsync(new SqlCommand(queryResult.Statement, queryResult.Parameters, timeout));
                }
            }
            return result;
        }

        #endregion

        #region Retrieve

        public T Get<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            return Select<T>(sql, parameters, timeout).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            var result = await SelectAsync<T>(sql, parameters, timeout);

            if (result == null)
                return null;

            return result.FirstOrDefault();
        }

        public IList<T> Select<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            using (IOrm orm = this.orm)
            {
                return orm.Select<T>(new SqlCommand(sql, parameters, timeout), CustomMapper.Resolve<T>()).ToList();
            }
        }

        public async Task<IList<T>> SelectAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            IEnumerable<T> result = null;
            using (IOrm orm = this.orm)
            {
                result = await orm.SelectAsync<T>(new SqlCommand(sql, parameters, timeout), CustomMapper.Resolve<T>());
            }

            if (result == null)
                return null;

            return result.ToList();
        }

        public T Get<T>(Expression<Func<T, bool>> expression, int timeout = 15) where T : class, new()
        {
            return Select<T>(expression, timeout).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15) where T : class, new()
        {
            var result = await SelectAsync<T>(expression, timeout);

            if (result == null)
                return null;

            return result.FirstOrDefault();
        }

        public IList<T> Select<T>(Expression<Func<T, bool>> expression, int timeout = 15) where T : class, new()
        {
            using (IOrm orm = this.orm)
            {
                SqlCommand result = DynamicQuery.GetDynamicQuery(typeof(T).Name.ToLowerInvariant().Pluralize(), expression);
                return orm.Select<T>(new SqlCommand(result.Statement, result.Parameters, timeout)).ToList();
            }
        }

        public async Task<IList<T>> SelectAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15) where T : class, new()
        {
            IEnumerable<T> result = null;
            using (IOrm orm = this.orm)
            {
                SqlCommand query = DynamicQuery.GetDynamicQuery(typeof(T).Name.ToLowerInvariant().Pluralize(), expression);
                result = await orm.SelectAsync<T>(new SqlCommand(query.Statement, query.Parameters, timeout));
            }

            if (result == null)
                return null;

            return result.ToList();
        }

        #endregion

        #region Update

        public bool Update<T>(T entity, int timeout = 15) where T : class, new()
        {
            if (entity == null) return false;
            SqlCommand queryResult = DynamicQuery.GetUpdateQuery(typeof(T).Name.ToLowerInvariant().Pluralize(), entity);
            using (IOrm orm = this.orm)
            {
                return orm.Execute(new SqlCommand(queryResult.Statement, queryResult.Parameters, timeout));
            }
        }
        public async Task<bool> UpdateAsync<T>(T entity, int timeout = 15) where T : class, new()
        {
            var result = false;

            if (entity == null)
                return result;

            SqlCommand query = DynamicQuery.GetUpdateQuery(typeof(T).Name.ToLowerInvariant().Pluralize(), entity);

            using (IOrm orm = this.orm)
            {
                result = await orm.ExecuteAsync(new SqlCommand(query.Statement, query.Parameters, timeout));
            }

            return result;
        }

        public void ApplyUpdates<T>(IEnumerable<T> items, IList<T> deletedItems, Func<T, bool> insertCriterium, Func<T, bool> updateCriterium, int timeout)
            where T : class, new()
        {
            if (deletedItems != null)
            {
                foreach (T item in deletedItems)
                {
                    Delete<T>(item, timeout);
                }
                deletedItems.Clear();
            }

            if (items != null)
            {
                foreach (T item in items)
                {
                    if (insertCriterium(item))
                    {
                        Insert<T>(item, timeout);
                    }
                    else if (updateCriterium(item))
                    {
                        Update<T>(item, timeout);
                    }
                }
            }
        }

        #endregion

        #region Delete

        public bool Delete<T>(T entity, int timeout = 15) where T : class, new()
        {
            if (entity == null) return false;
            SqlCommand queryResult = DynamicQuery.GetDeleteQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), entity);
            using (IOrm orm = this.orm)
            {
                return orm.Execute(new SqlCommand(queryResult.Statement, queryResult.Parameters, timeout));
            }
        }
        public async Task<bool> DeleteAsync<T>(T entity, int timeout = 15) where T : class, new()
        {
            var result = false;

            if (entity == null)
                return false;

            SqlCommand queryResult = DynamicQuery.GetDeleteQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), entity);

            using (IOrm orm = this.orm)
            {
                result = await orm.ExecuteAsync(new SqlCommand(queryResult.Statement, queryResult.Parameters, timeout));
            }

            return result;
        }

        #endregion
    }
}
