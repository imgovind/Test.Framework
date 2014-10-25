using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Cache.InProc
{
    public static class InProcRegister
    {
        public static void Initialize()
        {
            Container.Register<ICacher, InProcCacher>();
        }
    }
}
