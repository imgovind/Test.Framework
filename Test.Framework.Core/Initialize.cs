using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Cache;

namespace Test.Framework
{
    public static class Initalize
    {
        public static void Base()
        {
            Container.InitializeWith(new LightInjectTypeResolver());
            Container.Register<IWebConfiguration, WebConfiguration>(ObjectLifeSpans.Singleton);
        }

        public static void Cache()
        { 
            InProcCacher.InitializeWith(Container.Resolve<ICacher>(FrameworkSettings.IocKeys.InProcCacherKey));
            OutProcCacher.InitializeWith(Container.Resolve<ICacher>(FrameworkSettings.IocKeys.OutProcCacherKey));
        }
    }
}
