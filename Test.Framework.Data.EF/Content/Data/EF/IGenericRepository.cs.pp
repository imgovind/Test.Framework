using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace $rootnamespace$.Data.EF
{
    public interface IGenericRepository
    {
        IQueryable<T> GetAll<T>()
            where T : class;
        IQueryable<T> FindBy<T>(Expression<Func<T, bool>> predicate)
            where T : class;
        void Add<T>(T entity)
            where T : class;
        void Delete<T>(T entity)
            where T : class;
        void Edit<T>(T entity)
            where T : class;
        void Save();
    }
}
