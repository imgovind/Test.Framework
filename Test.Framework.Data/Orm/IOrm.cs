using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Data
{
    public interface IOrm : IDisposable
    {
        IDbConnection GetConnection();

        bool Execute(SqlDbCommand query);

        Task<bool> ExecuteAsync(SqlDbCommand query);

        bool Execute(IList<SqlDbCommand> queries);

        Task<bool> ExecuteAsync(IList<SqlDbCommand> queries);

        IEnumerable<T> Select<T>(SqlDbCommand query, ISelectable<T> traits = null)
            where T : class, new();

        Task<IEnumerable<T>> SelectAsync<T>(SqlDbCommand query, ISelectable<T> traits = null)
            where T : class, new();

        IEnumerable<T> Select<T>(SqlDbCommand query, Func<DataReader, T> readMapper)
            where T : class, new();

        Task<IEnumerable<T>> SelectAsync<T>(SqlDbCommand query, Func<DataReader, T> readMapper)
            where T : class, new();
    }
}
