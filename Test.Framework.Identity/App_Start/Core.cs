using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;

namespace Test.Framework.Identity
{
    public partial class AppInitializer
    {
        public partial class Core
        {
            public static void Initialize()
            {
                Container.InitializeWith(new LightInjectTypeResolver());
                Container.Register<IWebConfiguration, WebConfiguration>(ObjectLifeSpans.Singleton);
            }
        }
    }
}
