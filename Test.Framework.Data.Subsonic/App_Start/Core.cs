using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;

namespace Test.Framework.Data.Subsonic
{
    public partial class AppInitalizer
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
