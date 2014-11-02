using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $rootnamespace$.Models.EF;

namespace $rootnamespace$.Data.EF
{
    public interface IFooRepository : IGenericRepository
    {
        Foo GetSingle(int fooId);
        void Create(Foo foo);
        void Remove(Foo foo);
        void Update(Foo foo);
    }
}
