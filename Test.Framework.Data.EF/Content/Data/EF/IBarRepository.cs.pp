using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $rootnamespace$.Models.EF;

namespace $rootnamespace$.Data.EF
{
    public interface IBarRepository : IGenericRepository
    {
        Bar GetSingle(int barId);
    }
}
