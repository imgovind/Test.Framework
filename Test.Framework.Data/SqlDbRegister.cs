using LightInject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Data
{
    public static class SqlDbRegister
    {
        public static void Register(IEnumerable<string> connectionNames, SqlDbmsType sqlDbmsType = SqlDbmsType.MySql)
        {
            switch (sqlDbmsType)
            {
                case SqlDbmsType.SqlServer:
                case SqlDbmsType.MySql:
                case SqlDbmsType.Oracle:
                case SqlDbmsType.SqlLite:
                case SqlDbmsType.PostGreSql:
                default:
                    DbRegisterLightInject(connectionNames);
                    break;
            }
        }

        private static void DbRegisterIoc(IEnumerable<string> connectionNames)
        {
            connectionNames.ToList().ForEach(connectionName =>
            {
                Container.RegisterInstance<IDatabase, Database>(
                    connectionName,
                    new Database(Container.Resolve<IOrm>(connectionName)),
                    ObjectLifeSpans.Singleton);
            });
        }

        private static void DbRegisterLightInject(IEnumerable<string> connectionNames)
        {
            connectionNames.ToList().ForEach(connectionName =>
            {
                var container = (ServiceContainer)Container.IocContainer.GetUnderlyingContainer();
                container.Register<IDatabase>(factory => 
                    new Database(Container.Resolve<IOrm>(connectionName)), 
                    connectionName, 
                    new PerContainerLifetime());
            });
        }
    }
}
