using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $rootnamespace$.Data.EF
{
    public partial class ConnectionConstants
    {
        public static string Foo { get { return "FooConnectionString"; } }
        public static string Bar { get { return "BarConnectionString"; } }
		public static string User { get { return "mysql:AuthenticationConnectionString"; } }
    }
}
