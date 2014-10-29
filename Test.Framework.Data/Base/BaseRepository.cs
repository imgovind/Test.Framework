using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Test.Framework.Extensions;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Test.Framework.Data
{
    public abstract class BaseRepository : IRepository
    {
        public IDatabase Database { get; private set; }

        #region Private Members and Constructors
        
        public BaseRepository(string connectionName)
        {
            Ensure.Argument.IsNotEmpty(connectionName, "connectionName");

            var dbConnection = Container.Resolve<IDbConnection>(connectionName);
            var dataAccessConnection = Container.Resolve<IOrm>(connectionName);
            this.Database = new Database(dbConnection, dataAccessConnection);
        }

        public BaseRepository(IDatabase Database)
        {
            Ensure.Argument.IsNotNull(Database, "database");

            this.Database = Database;
        }

        #endregion

        #region IRepository Members

        public virtual bool Add<T>(T entity) where T : class, new()
        {
            return this.Database.Insert<T>(entity);
        }

        public async virtual Task<bool> AddAsync<T>(T entity) where T : class, new()
        {
            var result = await this.Database.InsertAsync<T>(entity);
            return result;
        }

        public virtual bool Update<T>(T entity) where T : class, new()
        {
            return this.Database.Update<T>(entity);
        }

        public async virtual Task<bool> UpdateAsync<T>(T entity) where T : class, new()
        {
            var result = await this.Database.UpdateAsync<T>(entity);
            return result;
        }

        public virtual bool AddMany<T>(IEnumerable<T> list) where T : class, new()
        {
            try
            {
                list.ForEach(item =>
                {
                    this.Database.Insert<T>(item);
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async virtual Task<bool> AddManyAsync<T>(IEnumerable<T> list) where T : class, new()
        {
            List<Task<bool>> inserts = new List<Task<bool>>();
            try
            {
                list.ForEach(item => inserts.Add(this.Database.InsertAsync<T>(item)));
                var outputs = await Task.WhenAll(inserts);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual T Get<T>(string sql) where T : class, new()
        {
            return this.Database.Get<T>(sql);
        }

        public async virtual Task<T> GetAsync<T>(string sql) where T : class, new()
        {
            var result = await this.Database.GetAsync<T>(sql);
            return result;
        }

        public virtual T Get<T>(string sql, IList<Parameter> parameters) where T : class, new()
        {
            return this.Database.Get<T>(sql, parameters);
        }

        public async virtual Task<T> GetAsync<T>(string sql, IList<Parameter> parameters) where T : class, new()
        {
            var result = await this.Database.GetAsync<T>(sql, parameters);
            return result;
        }

        public virtual T Get<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return this.Database.Get<T>(expression);
        }

        public async virtual Task<T> GetAsync<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            var result = await this.Database.GetAsync<T>(expression);
            return result;
        }

        public virtual IList<T> Find<T>(string sql) where T : class, new()
        {
            return this.Database.Select<T>(sql);
        }

        public async virtual Task<IList<T>> FindAsync<T>(string sql) where T : class, new()
        {
            var result = await this.Database.SelectAsync<T>(sql);
            return result;
        }

        public virtual IList<T> Find<T>(string sql, IList<Parameter> parameters) where T : class, new()
        {
            return this.Database.Select<T>(sql, parameters);
        }

        public async virtual Task<IList<T>> FindAsync<T>(string sql, IList<Parameter> parameters) where T : class, new()
        {
            var result = await this.Database.SelectAsync<T>(sql, parameters);
            return result;
        }

        public virtual IList<T> Find<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return this.Database.Select<T>(expression);
        }

        public async virtual Task<IList<T>> FindAsync<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            var result = await this.Database.SelectAsync<T>(expression);
            return result;
        }

        #endregion
    }
}
