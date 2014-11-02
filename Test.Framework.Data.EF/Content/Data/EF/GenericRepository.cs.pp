using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Test.Framework;
using System.Threading.Tasks;

namespace $rootnamespace$.Data.EF
{
    public abstract class GenericRepository<C> : IGenericRepository
        where C : DbContext
    {
        public C Context { get; set; }

        public GenericRepository(C Context)
        {
            Ensure.Argument.IsNotNull(Context, "Context");
            this.Context = Context;
        }

        public virtual IQueryable<T> GetAll<T>()
            where T : class
        {
            IQueryable<T> query = Context.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {

            IQueryable<T> query = Context.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Add<T>(T entity)
            where T : class
        {
            Context.Set<T>().Add(entity);
        }

        public virtual void Delete<T>(T entity)
            where T : class
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual void Edit<T>(T entity)
            where T : class
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }
    }
}
