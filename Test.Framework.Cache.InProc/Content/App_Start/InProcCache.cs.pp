using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Cache;
using Test.Framework.Cache.InProc;

namespace $rootnamespace$
{
    public partial class AppInitializer
    {
        public partial class InProcCache
        {
            public static void Register()
            {
                Container.Register<ICacher, InProcCacher>(FrameworkSettings.IocKeys.InProcCacherKey);
            }

            public static void Initialize()
            {
                Cacher.InitializeInProc(Container.Resolve<ICacher>(FrameworkSettings.IocKeys.InProcCacherKey));
            }
        }
    }
}
