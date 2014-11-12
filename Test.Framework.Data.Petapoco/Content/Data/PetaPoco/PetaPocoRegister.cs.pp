using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;

namespace $rootnamespace$.Data.Petapoco
{
    public partial class PetaPocoRegister
    {
        public static void Register(IEnumerable<string> connectionStringNames)
        {
            foreach (var connectionStringName in connectionStringNames)
            {
                RegisterPetaPoco(connectionStringName);
            }
        }

        private static void RegisterPetaPoco(string connectionString)
        {
            object[] args = new object[] { connectionString };
            var instance = Activator.CreateInstance(typeof(PetaPocoDatabase), args) as PetaPocoDatabase;
            Container.RegisterInstance<PetaPocoDatabase>(connectionString, instance);
        }
    }
}
