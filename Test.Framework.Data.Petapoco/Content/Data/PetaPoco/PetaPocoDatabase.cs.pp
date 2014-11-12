using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;
using Test.Framework.Extensions;
using $rootnamespace$.Models;

namespace $rootnamespace$.Data.Petapoco
{
	public class PetaPocoDatabase : PetaPoco.Database, IDatabase
    {
        private string connectionString;
        public PetaPocoDatabase(string connectionName)
            : base(connectionName)
        {
            this.connectionString = connectionName;
        }

        public string ConnectionString
        {
            get { return this.connectionString; }
        }

        public OrmType DbOrmType
        {
            get { return OrmType.PetaPoco; }
        }

        #region Delete Methods

        public bool Delete<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new()
        {
            SqlDbCommand command = DynamicQuery.GetDeleteQuery<T>(expression, timeout, this.DbOrmType);
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public bool Delete<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null) where T : class, new()
        {
            SqlDbCommand command = DynamicQuery.GetDeleteQuery<T>(entity, primaryKeyColumn, timeout, this.DbOrmType);
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public bool Delete<T>(T entity, int timeout = 15, string tableName = null) where T : class, new()
        {
            SqlDbCommand command = DynamicQuery.GetDeleteQuery<T>(entity, timeout, this.DbOrmType);
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public async Task<bool> DeleteAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new()
        {
            var result = await Task.Run(() => Delete<T>(expression, timeout, tableName));
            return result;
        }

        public async Task<bool> DeleteAsync<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null) where T : class, new()
        {
            var result = await Task.Run(() => Delete<T>(entity, primaryKeyColumn, timeout, tableName));
            return result;
        }

        public async Task<bool> DeleteAsync<T>(T entity, int timeout = 15, string tableName = null) where T : class, new()
        {
            var result = await Task.Run(() => Delete<T>(entity, timeout, tableName));
            return result;
        }

        #endregion

        #region Deprecate Methods

        public bool Deprecate<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new()
        {
            SqlDbCommand command = DynamicQuery.GetDeprecateQuery<T>(expression, timeout, this.DbOrmType);
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public bool Deprecate<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null) where T : class, new()
        {
            SqlDbCommand command = DynamicQuery.GetDeprecateQuery<T>(entity, primaryKeyColumn, timeout, this.DbOrmType);
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public bool Deprecate<T>(T entity, int timeout = 15, string tableName = null) where T : class, new()
        {
            SqlDbCommand command = DynamicQuery.GetDeprecateQuery<T>(entity, timeout, this.DbOrmType);
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public async Task<bool> DeprecateAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new()
        {
            var result = await Task.Run(() => Deprecate<T>(expression, timeout, tableName));
            return result;
        }

        public async Task<bool> DeprecateAsync<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null) where T : class, new()
        {
            var result = await Task.Run(() => Deprecate<T>(entity, primaryKeyColumn, timeout, tableName));
            return result;
        }

        public async Task<bool> DeprecateAsync<T>(T entity, int timeout = 15, string tableName = null) where T : class, new()
        {
            var result = await Task.Run(() => Deprecate<T>(entity, timeout, tableName));
            return result;
        }

        #endregion

        #region Execute Methods

        public bool Execute(IList<SqlDbCommand> unitOfWorks)
        {
            using (var scope = base.GetTransaction())
            {
                try
                {
                    foreach (var unitOfWork in unitOfWorks)
                    {
                        this.Execute(unitOfWork.Statement, unitOfWork.Parameters.ToObjectArray(true));
                    }
                    base.CompleteTransaction();
                    return true;
                }
                catch (Exception)
                {
                    base.AbortTransaction();
                    return false;
                }
            }
        }

        public bool Execute<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            return this.Execute(sql, parameters.ToObjectArray(true)) > 0;
        }

        public async Task<bool> ExecuteAsync(IList<SqlDbCommand> unitOfWorks)
        {
            var result = await Task.Run(() => Execute(unitOfWorks));
            return result;
        }

        public async Task<bool> ExecuteAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            var result = await Task.Run(() => Execute<T>(sql, parameters, timeout));
            return result;
        }

        #endregion

        #region Retrieve Methods
        public T Get<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new()
        {
            return Select<T>(expression, timeout, tableName).FirstOrDefault();
        }

        public T Get<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            return Select<T>(sql, parameters, timeout).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new()
        {
            var result = await SelectAsync<T>(expression, timeout, tableName);
            if (result == null) return null;
            return result.FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            var result = await SelectAsync<T>(sql, parameters, timeout);
            if (result == null) return null;
            return result.FirstOrDefault();
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new()
        {
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetSelectQuery<T>(timeout, expression, this.DbOrmType);
            else
                command = DynamicQuery.GetSelectQuery<T>(tableName, expression, timeout, this.DbOrmType);
            if (command == null) return null;
            return this.Query<T>(command.Statement, command.Parameters.ToObjectArray(true));
        }

        public IEnumerable<T> Select<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            return this.Query<T>(sql, parameters.ToObjectArray(true));
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new()
        {
            var result = await Task.Run(() => Select<T>(expression, timeout, tableName));
            return result; ;
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            var result = await Task.Run(() => Select<T>(sql, parameters, timeout));
            return result;
        }

        public IEnumerable<T> Page<T>(int take = 50, int skip = 0, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetPageQuery<T>(take, skip, timeout, this.DbOrmType);
            else
                command = DynamicQuery.GetPageQuery<T>(tableName, null, take, skip, timeout);
            if (command == null) return null;
            return this.Query<T>(command.Statement, command.Parameters.ToObjectArray(true));
        }

        public async Task<IEnumerable<T>> PageAsync<T>(int take = 50, int skip = 0, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            var result = await Task.Run(() => Page<T>(take, skip, timeout, tableName));
            return result;
        }

        public IEnumerable<T> Page<T>(Expression<Func<T, bool>> expression = null, int take = 50, int skip = 0, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetPageQuery<T>(expression, take, skip, timeout, this.DbOrmType);
            else
                command = DynamicQuery.GetPageQuery<T>(tableName, expression, take, skip, timeout, this.DbOrmType);
            if (command == null) return null;
            return this.Query<T>(command.Statement, command.Parameters.ToObjectArray(true));
        }

        public async Task<IEnumerable<T>> PageAsync<T>(Expression<Func<T, bool>> expression = null, int take = 50, int skip = 0, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            var result = await Task.Run(() => Page<T>(expression, take, skip, timeout, tableName));
            return result;
        }

        #endregion

        #region Insert Methods
        public bool Insert<T>(T entity, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetInsertQuery<T>(entity, timeout, this.DbOrmType);
            else
                command = DynamicQuery.GetInsertQuery<T>(tableName, entity, false, null, timeout, this.DbOrmType);
            if (command == null) return false;
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public async Task<bool> InsertAsync<T>(T entity, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            var result = await Task.Run(() => Insert<T>(entity, timeout, tableName));
            return result;
        }

        public bool Insert<T>(T entity, bool IsAutoIncrement, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetInsertQuery<T>(entity, IsAutoIncrement, null, timeout, this.DbOrmType);
            else
                command = DynamicQuery.GetInsertQuery<T>(tableName, entity, IsAutoIncrement, null, timeout, this.DbOrmType);
            if (command == null) return false;
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public async Task<bool> InsertAsync<T>(T entity, bool IsAutoIncrement, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            var result = await Task.Run(() => Insert<T>(entity, IsAutoIncrement, timeout, tableName));
            return result; ;
        }

        public bool Insert<T>(T entity, bool IsAutoIncrement, string primaryKeyColumn, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetInsertQuery<T>(entity, IsAutoIncrement, primaryKeyColumn, timeout, this.DbOrmType);
            else
                command = DynamicQuery.GetInsertQuery<T>(tableName, entity, IsAutoIncrement, primaryKeyColumn, timeout, this.DbOrmType);
            if (command == null) return false;
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public async Task<bool> InsertAsync<T>(T entity, bool IsAutoIncrement, string primaryKeyColumn, int timeout = 15, string tableName = null)
            where T : class, new()
        {
            var result = await Task.Run(() => Insert<T>(entity, IsAutoIncrement, primaryKeyColumn, timeout, tableName));
            return result; ;
        }
        #endregion

        #region Update Methods
        public bool Update<T>(T entity, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetUpdateQuery<T>(entity, timeout, excludedColumns, this.DbOrmType);
            else
                command = DynamicQuery.GetUpdateQuery<T>(tableName, entity, this.DbOrmType, null, null, timeout, excludedColumns);
            if (command == null) return false;
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public async Task<bool> UpdateAsync<T>(T entity, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            var result = await Task.Run(() => Update<T>(entity, timeout, excludedColumns, tableName));
            return result;
        }

        public bool Update<T>(T entity, string primaryKeyColumn, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetUpdateQuery<T>(entity, primaryKeyColumn, timeout, excludedColumns, this.DbOrmType);
            else
                command = DynamicQuery.GetUpdateQuery<T>(tableName, entity, this.DbOrmType, primaryKeyColumn, null, timeout, excludedColumns);
            if (command == null) return false;
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public async Task<bool> UpdateAsync<T>(T entity, string primaryKeyColumn, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            var result = await Task.Run(() => Update<T>(entity, primaryKeyColumn, timeout, excludedColumns, tableName));
            return result;
        }

        public bool Update<T>(T entity, Expression<Func<T, bool>> expression, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            if (entity == null) return false;
            SqlDbCommand command = null;
            if (tableName.IsNullOrEmpty())
                command = DynamicQuery.GetUpdateQuery<T>(entity, expression, timeout, excludedColumns, this.DbOrmType);
            else
                command = DynamicQuery.GetUpdateQuery<T>(tableName, entity, this.DbOrmType, null, expression, timeout, excludedColumns);
            if (command == null) return false;
            return this.Execute(command.Statement, command.Parameters.ToObjectArray(true)) > 0;
        }

        public async Task<bool> UpdateAsync<T>(T entity, Expression<Func<T, bool>> expression, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null)
            where T : class, new()
        {
            var result = await Task.Run(() => Update<T>(entity, expression, timeout, excludedColumns, tableName));
            return result;
        }
        #endregion
    }
}