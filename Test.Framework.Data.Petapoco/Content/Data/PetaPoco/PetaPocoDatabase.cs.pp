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

        public void ApplyUpdates<T>(IEnumerable<T> items, IList<T> deletedItems, Func<T, bool> insertCriterium, Func<T, bool> updateCriterium, int timeout) 
            where T : class, new()
        {
            throw new NotImplementedException();
        }

        #region Insert Methods
        public bool Insert<T>(T entity, int timeout = 15) where T : class, new()
        {
            return this.Insert(entity);
        }

        public async Task<bool> InsertAsync<T>(T entity, int timeout = 15) 
            where T : class, new()
        {
            var result = await Task.Run(() => Insert<T>(entity));
            return result;
        } 
        #endregion

        #region Retrieve Methods
        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression = null)
            where T : class
        {
            var query = DynamicQuery.GetDynamicQuery<T>(expression, true);
            return this.Query<T>(query.Statement, query.Parameters.ToObjectArray(true));
        }

        public IEnumerable<T> Select<T>(string sql, IList<Parameter> parameters = null, int timeout = 15)
            where T : class, new()
        {
            return this.Query<T>(sql, parameters.ToObjectArray(true));
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15)
            where T : class, new()
        {
            var result = await Task.Run(() => Select<T>(expression));
            return result;
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, int timeout = 15)
            where T : class, new()
        {
            return Select<T>(expression);
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15)
            where T : class, new()
        {
            var result = await Task.Run(() => Select<T>(sql, parameters, timeout));
            return result;
        }
        public T Get<T>(Expression<Func<T, bool>> expression, int timeout = 15)
            where T : class, new()
        {
            return this.Select<T>(expression).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15)
            where T : class, new()
        {
            var result = await Task.Run(() => Get<T>(expression));
            return result;
        }

        public T Get<T>(string sql, IList<Parameter> parameters = null, int timeout = 15)
            where T : class, new()
        {
            return this.Query<T>(sql, parameters.ToObjectArray(true)).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15)
            where T : class, new()
        {
            var result = await Task.Run(() => Get<T>(sql, parameters, timeout));
            return result;
        }
        
        #endregion

        #region Update Methods
        public bool Update<T>(T entity, int timeout = 15) 
            where T : class, new()
        {
            return this.Update(entity);
        }

        public async Task<bool> UpdateAsync<T>(T entity, int timeout = 15) 
            where T : class, new()
        {
            var result = await Task.Run(() => Update<T>(entity));
            return result;
        } 
        #endregion

        #region Delete Methods
        public bool Delete<T>(T entity, int timeout = 15) 
            where T : class, new()
        {
            return base.Delete(entity) > 0;
        }

        public async Task<bool> DeleteAsync<T>(T entity, int timeout = 15) 
            where T : class, new()
        {
            var result = await Task.Run(() => Delete<T>(entity));
            return result;
        } 
        #endregion

        #region Execute Methods
        public bool Execute(IList<SqlDbCommand> unitOfWorks)
        {
            if (unitOfWorks == null ||
                !unitOfWorks.Any())
                return false;

            try
            {
                base.BeginTransaction();
                foreach (var sqlCommand in unitOfWorks)
                {
                    this.Execute(sqlCommand.Statement, sqlCommand.Parameters.ToObjectArray(true));
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

        public async Task<bool> ExecuteAsync(IList<SqlDbCommand> unitOfWorks)
        {
            var result = await Task.Run(() => Execute(unitOfWorks));
            return result;
        }

        public bool Execute<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            return this.Execute(sql, parameters.ToObjectArray(true)) > 0;
        }

        public async Task<bool> ExecuteAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new()
        {
            var result = await Task.Run(() => Execute<T>(sql, parameters, timeout));
            return result;
        } 
        #endregion

    }
}
