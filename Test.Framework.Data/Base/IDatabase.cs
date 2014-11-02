using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Test.Framework.Data
{
    public interface IDatabase
    {
        string ConnectionString { get; }
        IDbConnection Connection { get; }

        bool Execute<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        Task<bool> ExecuteAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        bool Execute(IList<SqlDbCommand> unitOfWorks);
        Task<bool> ExecuteAsync(IList<SqlDbCommand> unitOfWorks);

        bool Insert<T>(T entity, int timeout = 15) where T : class, new();
        Task<bool> InsertAsync<T>(T entity, int timeout = 15) where T : class, new();

        T Get<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        Task<T> GetAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        IList<T> Select<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        Task<IList<T>> SelectAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        T Get<T>(Expression<Func<T, bool>> expression, int timeout = 15) where T : class, new();
        Task<T> GetAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15) where T : class, new();
        IList<T> Select<T>(Expression<Func<T, bool>> expression, int timeout = 15) where T : class, new();
        Task<IList<T>> SelectAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15) where T : class, new();

        bool Update<T>(T entity, int timeout = 15) where T : class, new();
        Task<bool> UpdateAsync<T>(T entity, int timeout = 15) where T : class, new();
        void ApplyUpdates<T>(IEnumerable<T> items, IList<T> deletedItems, Func<T, bool> insertCriterium, Func<T, bool> updateCriterium, int timeout) where T : class, new();

        bool Delete<T>(T entity, int timeout = 15) where T : class, new();
        Task<bool> DeleteAsync<T>(T entity, int timeout = 15) where T : class, new();
    }
}
