using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;

namespace $rootnamespace$
{
    public partial class AppInitalizer
    {
        public partial class Core
        {
            public static void Initialize()
            {
                Container.InitializeWith(new MunqTypeResolver());
                Container.Register<IWebConfiguration, WebConfiguration>(ObjectLifeSpans.Singleton);
            }
        }
    }
}
