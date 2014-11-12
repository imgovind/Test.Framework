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
        public IOrm orm { get; set; }
        public OrmType DbOrmType
        {
            get 
            {
                return OrmType.Dapper;
            }
        }

        public Database(IOrm orm)
        {
            Ensure.Argument.IsNotNull(orm, "orm");
            this.orm = orm;
        }

        #region Execute

        public bool Execute<T>(string sql, IList<Parameter> parameters = null, int timeout = 15)
            where T : class, new()
        {
            using (IOrm orm = this.orm)
            {
                return orm.Execute(new SqlDbCommand(sql, parameters, timeout));
            }
        }

        public async Task<bool> ExecuteAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15)
            where T : class, new()
        {
            var result = false;
            using (IOrm orm = this.orm)
            {
                result = await orm.ExecuteAsync(new SqlDbCommand(sql, parameters, timeout));
                return result;
            }
        }

        #endregion

        #region Transaction

        public bool Execute(IList<SqlDbCommand> unitOfWorks)
        {
            using (IOrm orm = this.orm)
            {
                return orm.Execute(unitOfWorks);
            }
        }

        public async Task<bool> ExecuteAsync(IList<SqlDbCommand> unitOfWorks)
        {
            var result = false;
            using (IOrm orm = this.orm)
            {
                result = await orm.ExecuteAsync(unitOfWorks);
                return result;
            }
        }

        #endregion

        #region Create

        public bool Insert<T>(T entity, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetInsertQuery<T>(entity, timeout);
            else
                command = DynamicQuery.GetInsertQuery<T>(tableName, entity, false, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> InsertAsync<T>(T entity, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetInsertQuery<T>(entity, timeout);
            else
                command = DynamicQuery.GetInsertQuery<T>(tableName, entity, false, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        public bool Insert<T>(T entity, bool IsAutoIncrement, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetInsertQuery<T>(entity, IsAutoIncrement, null, timeout);
            else
                command = DynamicQuery.GetInsertQuery<T>(tableName, entity, IsAutoIncrement, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> InsertAsync<T>(T entity, bool IsAutoIncrement, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetInsertQuery<T>(entity, IsAutoIncrement, null, timeout);
            else
                command = DynamicQuery.GetInsertQuery<T>(tableName, entity, IsAutoIncrement, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        public bool Insert<T>(T entity, bool IsAutoIncrement, string primaryKeyColumn, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetInsertQuery<T>(entity, IsAutoIncrement, primaryKeyColumn, timeout);
            else
                command = DynamicQuery.GetInsertQuery<T>(tableName, entity, IsAutoIncrement, primaryKeyColumn, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> InsertAsync<T>(T entity, bool IsAutoIncrement, string primaryKeyColumn, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetInsertQuery<T>(entity, IsAutoIncrement, primaryKeyColumn, timeout);
            else
                command = DynamicQuery.GetInsertQuery<T>(tableName, entity, IsAutoIncrement, primaryKeyColumn, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        #endregion

        #region Retrieve

        public T Get<T>(string sql, IList<Parameter> parameters = null, int timeout = 15)
            where T : class, new()
        {
            return Select<T>(sql, parameters, timeout).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15)
            where T : class, new()
        {
            var result = await SelectAsync<T>(sql, parameters, timeout);
            if (result == null) return null;
            return result.FirstOrDefault();
        }

        public T Get<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            return Select<T>(expression, timeout, tableName).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            var result = await SelectAsync<T>(expression, timeout, tableName);
            if (result == null) return null;
            return result.FirstOrDefault();
        }

        public IEnumerable<T> Select<T>(string sql, IList<Parameter> parameters = null, int timeout = 15)
            where T : class, new()
        {
            using (IOrm orm = this.orm)
            {
                return orm.Select<T>(new SqlDbCommand(sql, parameters, timeout), CustomMapper.Resolve<T>()).ToList();
            }
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15)
            where T : class, new()
        {
            IEnumerable<T> result = null;
            using (IOrm orm = this.orm)
            {
                result = await orm.SelectAsync<T>(new SqlDbCommand(sql, parameters, timeout), CustomMapper.Resolve<T>());
                if (result == null) return null;
                return result.ToList();
            }
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression = null, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetSelectQuery<T>(timeout, expression);
            else
                command = DynamicQuery.GetSelectQuery<T>(tableName, expression, timeout);
            if (command == null) return null;
            using (IOrm orm = this.orm)
            {
                return orm.Select<T>(command).ToList();
            }
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(Expression<Func<T, bool>> expression = null, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            IEnumerable<T> result = null;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetSelectQuery<T>(timeout, expression);
            else
                command = DynamicQuery.GetSelectQuery<T>(tableName, expression, timeout);
            if (command == null) return null;
            using (IOrm orm = this.orm)
            {
                result = await orm.SelectAsync<T>(command);
                if (result == null) return null;
                return result.ToList();
            }
        }

        public IEnumerable<T> Page<T>(int take = 50, int skip = 0, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetPageQuery<T>(take, skip, timeout);
            else
                command = DynamicQuery.GetPageQuery<T>(tableName, null, take, skip, timeout);
            if (command == null) return null;
            using (IOrm orm = this.orm)
            {
                return orm.Select<T>(command).ToList();
            }
        }

        public async Task<IEnumerable<T>> PageAsync<T>(int take = 50, int skip = 0, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            IEnumerable<T> result = null;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetPageQuery<T>(take, skip, timeout);
            else
                command = DynamicQuery.GetPageQuery<T>(tableName, null, take, skip, timeout);
            if (command == null) return null;
            using (IOrm orm = this.orm)
            {
                result = await orm.SelectAsync<T>(command);
                if (result == null) return null;
                return result.ToList();
            }
        }

        public IEnumerable<T> Page<T>(Expression<Func<T, bool>> expression = null, int take = 50, int skip = 0, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetPageQuery<T>(expression, take, skip, timeout);
            else
                command = DynamicQuery.GetPageQuery<T>(tableName, expression, take, skip, timeout);
            if (command == null) return null;
            using (IOrm orm = this.orm)
            {
                return orm.Select<T>(command).ToList();
            }
        }

        public async Task<IEnumerable<T>> PageAsync<T>(Expression<Func<T, bool>> expression = null, int take = 50, int skip = 0, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            IEnumerable<T> result = null;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetPageQuery<T>(expression, take, skip, timeout);
            else
                command = DynamicQuery.GetPageQuery<T>(tableName, expression, take, skip, timeout);
            if (command == null) return null;
            using (IOrm orm = this.orm)
            {
                result = await orm.SelectAsync<T>(command);
                if (result == null) return null;
                return result.ToList();
            }
        }

        #endregion

        #region Update

        public bool Update<T>(T entity, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetUpdateQuery<T>(entity, timeout, excludedColumns);
            else
                command = DynamicQuery.GetUpdateQuery<T>(tableName, entity, this.DbOrmType, null, null, timeout, excludedColumns);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> UpdateAsync<T>(T entity, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetUpdateQuery<T>(entity, timeout, excludedColumns);
            else
                command = DynamicQuery.GetUpdateQuery<T>(tableName, entity, this.DbOrmType, null, null, timeout, excludedColumns);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        public bool Update<T>(T entity, string primaryKeyColumn, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetUpdateQuery<T>(entity, primaryKeyColumn, timeout, excludedColumns);
            else
                command = DynamicQuery.GetUpdateQuery<T>(tableName, entity, this.DbOrmType, primaryKeyColumn, null, timeout, excludedColumns);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> UpdateAsync<T>(T entity, string primaryKeyColumn, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetUpdateQuery<T>(entity, primaryKeyColumn, timeout, excludedColumns);
            else
                command = DynamicQuery.GetUpdateQuery<T>(tableName, entity, this.DbOrmType, primaryKeyColumn, null, timeout, excludedColumns);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        public bool Update<T>(T entity, Expression<Func<T, bool>> expression, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetUpdateQuery<T>(entity, expression, timeout, excludedColumns);
            else
                command = DynamicQuery.GetUpdateQuery<T>(tableName, entity, this.DbOrmType, null, expression, timeout, excludedColumns);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> UpdateAsync<T>(T entity, Expression<Func<T, bool>> expression, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetUpdateQuery<T>(entity, expression, timeout, excludedColumns);
            else
                command = DynamicQuery.GetUpdateQuery<T>(tableName, entity, this.DbOrmType, null, expression, timeout, excludedColumns);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        #endregion

        #region Delete

        public bool Delete<T>(T entity, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeleteQuery<T>(entity, timeout);
            else
                command = DynamicQuery.GetDeleteQuery<T>(tableName, entity, this.DbOrmType, null, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> DeleteAsync<T>(T entity, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeleteQuery<T>(entity, timeout);
            else
                command = DynamicQuery.GetDeleteQuery<T>(tableName, entity, this.DbOrmType, null, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        public bool Delete<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeleteQuery<T>(entity, primaryKeyColumn, timeout);
            else
                command = DynamicQuery.GetDeleteQuery<T>(tableName, entity, this.DbOrmType, primaryKeyColumn, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> DeleteAsync<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeleteQuery<T>(entity, primaryKeyColumn, timeout);
            else
                command = DynamicQuery.GetDeleteQuery<T>(tableName, entity, this.DbOrmType, primaryKeyColumn, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        public bool Delete<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (expression == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeleteQuery<T>(expression, timeout);
            else
                command = DynamicQuery.GetDeleteQuery<T>(tableName, default(T), this.DbOrmType, null, expression, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> DeleteAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (expression == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeleteQuery<T>(expression, timeout);
            else
                command = DynamicQuery.GetDeleteQuery<T>(tableName, default(T), this.DbOrmType, null, expression, timeout);
            if (command == null) return false;
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        #endregion

        #region Deprecate

        public bool Deprecate<T>(T entity, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeprecateQuery<T>(entity, timeout);
            else
                command = DynamicQuery.GetDeprecateQuery<T>(tableName, entity, this.DbOrmType, null, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> DeprecateAsync<T>(T entity, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeprecateQuery<T>(entity, timeout);
            else
                command = DynamicQuery.GetDeprecateQuery<T>(tableName, entity, this.DbOrmType, null, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        public bool Deprecate<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeprecateQuery<T>(entity, primaryKeyColumn, timeout);
            else
                command = DynamicQuery.GetDeprecateQuery<T>(tableName, entity, this.DbOrmType, primaryKeyColumn, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> DeprecateAsync<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeprecateQuery<T>(entity, primaryKeyColumn, timeout);
            else
                command = DynamicQuery.GetDeprecateQuery<T>(tableName, entity, this.DbOrmType, primaryKeyColumn, null, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        public bool Deprecate<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (expression == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeprecateQuery<T>(expression, timeout);
            else
                command = DynamicQuery.GetDeprecateQuery<T>(tableName, default(T), this.DbOrmType, null, expression, timeout);
            if (command == null) return false;
            using (IOrm orm = this.orm)
            {
                return orm.Execute(command);
            }
        }

        public async Task<bool> DeprecateAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (expression == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetDeprecateQuery<T>(expression, timeout);
            else
                command = DynamicQuery.GetDeprecateQuery<T>(tableName, default(T), this.DbOrmType, null, expression, timeout);
            if (command == null) return false;
            using(IOrm orm = this.orm)
            {
                var result = await orm.ExecuteAsync(command);
                return result;
            }
        }

        #endregion

    }
}
