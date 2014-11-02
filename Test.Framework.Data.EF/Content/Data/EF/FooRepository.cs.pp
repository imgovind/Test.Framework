using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $rootnamespace$.Models.EF;

namespace $rootnamespace$.Data.EF
{
    public class FooRepository :
        GenericRepository<FooBarEntities>, IFooRepository
    {
        public FooRepository(FooBarEntities context)
            : base(context)
        {
        }

        public Foo GetSingle(int fooId)
        {

            var query = GetAll<Foo>().FirstOrDefault(x => x.FooId == fooId);
            return query;
        }

        public void Update(Foo foo)
        {
            Update(foo);
            Save();
        }

        public void Create(Foo foo)
        {
            Add(foo);
            Save();
        }

        public void Remove(Foo foo)
        {
            Delete(foo);
            Save();
        }
    }
}
