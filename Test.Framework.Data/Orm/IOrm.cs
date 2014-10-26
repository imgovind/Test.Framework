using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.DataAccess
{
    public interface IOrm : IDisposable
    {
        IDbConnection GetConnection();

        bool Execute(SqlCommand query);

        Task<bool> ExecuteAsync(SqlCommand query);

        bool Execute(IList<SqlCommand> queries);

        Task<bool> ExecuteAsync(IList<SqlCommand> queries);

        IEnumerable<T> Select<T>(SqlCommand query, ISelectable<T> traits = null)
            where T : class, new();

        Task<IEnumerable<T>> SelectAsync<T>(SqlCommand query, ISelectable<T> traits = null)
            where T : class, new();

        IEnumerable<T> Select<T>(SqlCommand query, Func<DataReader, T> readMapper)
            where T : class, new();

        Task<IEnumerable<T>> SelectAsync<T>(SqlCommand query, Func<DataReader, T> readMapper)
            where T : class, new();
    }
}
