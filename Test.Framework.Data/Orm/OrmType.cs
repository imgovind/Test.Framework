using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Data
{
    public enum OrmType
    {
        Custom = 1,
        Dapper = 2,
        PetaPoco = 3,
        EntityFramework = 4,
        SubSonic = 5
    }
}
