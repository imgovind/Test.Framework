using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test.Framework.DataAccess
{
    public interface IRepository
    {
        bool Add<T>(T entity) where T : class, new();
        Task<bool> AddAsync<T>(T entity) where T : class, new();
        bool Update<T>(T entity) where T : class, new();
        Task<bool> UpdateAsync<T>(T entity) where T : class, new();
        bool AddMany<T>(IEnumerable<T> entity) where T : class, new();
        Task<bool> AddManyAsync<T>(IEnumerable<T> entity) where T : class, new();

        T Get<T>(string sql) where T : class, new();
        Task<T> GetAsync<T>(string sql) where T : class, new();
        T Get<T>(string sql, IList<Parameter> parameters) where T : class, new();
        Task<T> GetAsync<T>(string sql, IList<Parameter> parameters) where T : class, new();
        T Get<T>(Expression<Func<T, bool>> expression) where T : class, new();
        Task<T> GetAsync<T>(Expression<Func<T, bool>> expression) where T : class, new();

        IList<T> Find<T>(string sql) where T : class, new();
        Task<IList<T>> FindAsync<T>(string sql) where T : class, new();
        IList<T> Find<T>(string sql, IList<Parameter> parameters) where T : class, new();
        Task<IList<T>> FindAsync<T>(string sql, IList<Parameter> parameters) where T : class, new();
        IList<T> Find<T>(Expression<Func<T, bool>> expression) where T : class, new();
        Task<IList<T>> FindAsync<T>(Expression<Func<T, bool>> expression) where T : class, new();
    }
}
