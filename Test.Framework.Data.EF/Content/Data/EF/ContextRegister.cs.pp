using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Test.Framework.Extensions;
using Test.Framework;

namespace $rootnamespace$.Data.EF
{
    public partial class EFRegister<TContext>
        where TContext : class
    {
        public static void RegisterContext(IEnumerable<string> connectionStringNames)
        {
            foreach (var connectionStringName in connectionStringNames)
            {
                RegisterContext(typeof(TContext), connectionStringName);
            }
        }

        private static void RegisterContext(Type dbContext, string connectionString)
        {
            object[] args = new object[] { connectionString };
            var instance = Activator.CreateInstance(dbContext, args) as TContext;
            Container.RegisterInstance<TContext>(connectionString, instance);
        }
    }

}
