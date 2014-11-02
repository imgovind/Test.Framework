using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $rootnamespace$.Models.EF;

namespace $rootnamespace$.Data.EF
{
    public class BarRepository :
        GenericRepository<FooBarEntities>, IBarRepository
    {

        public BarRepository(FooBarEntities context)
            : base(context)
        {
        }

        public Bar GetSingle(int barId)
        {

            var query = Context.Bars.FirstOrDefault(x => x.BarId == barId);
            return query;
        }
    }
}
