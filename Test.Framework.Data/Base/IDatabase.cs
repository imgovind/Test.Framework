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
        OrmType DbOrmType { get; }

        bool Execute<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        Task<bool> ExecuteAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        bool Execute(IList<SqlDbCommand> unitOfWorks);
        Task<bool> ExecuteAsync(IList<SqlDbCommand> unitOfWorks);

        bool Insert<T>(T entity, int timeout = 15, string tableName = null) where T : class, new();
        Task<bool> InsertAsync<T>(T entity, int timeout = 15, string tableName = null) where T : class, new();
        bool Insert<T>(T entity, bool IsAutoIncrement, int timeout = 15, string tableName = null) where T : class, new();
        Task<bool> InsertAsync<T>(T entity, bool IsAutoIncrement, int timeout = 15, string tableName = null) where T : class, new();
        bool Insert<T>(T entity, bool IsAutoIncrement, string primaryKeyColumn, int timeout = 15, string tableName = null) where T : class, new();
        Task<bool> InsertAsync<T>(T entity, bool IsAutoIncrement, string primaryKeyColumn, int timeout = 15, string tableName = null) where T : class, new();

        T Get<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        Task<T> GetAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        T Get<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new();
        Task<T> GetAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new();

        IEnumerable<T> Select<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        Task<IEnumerable<T>> SelectAsync<T>(string sql, IList<Parameter> parameters = null, int timeout = 15) where T : class, new();
        IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression = null, int timeout = 15, string tableName = null) where T : class, new();
        Task<IEnumerable<T>> SelectAsync<T>(Expression<Func<T, bool>> expression = null, int timeout = 15, string tableName = null) where T : class, new();
        IEnumerable<T> Page<T>(int take = 50, int skip = 0, int timeout = 15, string tableName = null) where T : class, new();
        Task<IEnumerable<T>> PageAsync<T>(int take = 50, int skip = 0, int timeout = 15, string tableName = null) where T : class, new();

        IEnumerable<T> Page<T>(Expression<Func<T, bool>> expression = null, int take = 50, int skip = 0, int timeout = 15, string tableName = null) where T : class, new();
        Task<IEnumerable<T>> PageAsync<T>(Expression<Func<T, bool>> expression = null, int take = 50, int skip = 0, int timeout = 15, string tableName = null) where T : class, new();

        bool Update<T>(T entity, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null) where T : class, new();
        Task<bool> UpdateAsync<T>(T entity, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null) where T : class, new();
        bool Update<T>(T entity, string primaryKeyColumn, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null) where T : class, new();
        Task<bool> UpdateAsync<T>(T entity, string primaryKeyColumn, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null) where T : class, new();
        bool Update<T>(T entity, Expression<Func<T,bool>> expression, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null) where T : class, new();
        Task<bool> UpdateAsync<T>(T entity, Expression<Func<T, bool>> expression, int timeout = 15, HashSet<string> excludedColumns = null, string tableName = null) where T : class, new();

        bool Delete<T>(T entity, int timeout = 15, string tableName = null) where T : class, new();
        Task<bool> DeleteAsync<T>(T entity, int timeout = 15, string tableName = null) where T : class, new();
        bool Delete<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null) where T : class, new();
        Task<bool> DeleteAsync<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null) where T : class, new();
        bool Delete<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new();
        Task<bool> DeleteAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new();

        bool Deprecate<T>(T entity, int timeout = 15, string tableName = null) where T : class, new();
        Task<bool> DeprecateAsync<T>(T entity, int timeout = 15, string tableName = null) where T : class, new();
        bool Deprecate<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null) where T : class, new();
        Task<bool> DeprecateAsync<T>(T entity, string primaryKeyColumn, int timeout = 15, string tableName = null) where T : class, new();
        bool Deprecate<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new();
        Task<bool> DeprecateAsync<T>(Expression<Func<T, bool>> expression, int timeout = 15, string tableName = null) where T : class, new();
    }
}
