using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;

namespace $rootnamespace$.Data.Subsonic
{
    public partial class SubSonicRegister
    {
        public static void Register(IEnumerable<string> connectionStringNames)
        {
            foreach (var connectionStringName in connectionStringNames)
            {
                RegisterSubSonic(connectionStringName);
            }
        }

        private static void RegisterSubSonic(string connectionString)
        {
            object[] args = new object[] { connectionString };
            var instance = Activator.CreateInstance(typeof(SubSonicDatabase), args) as SubSonicDatabase;
            Container.RegisterInstance<SubSonicDatabase>(connectionString, instance);
        }
    }
}
