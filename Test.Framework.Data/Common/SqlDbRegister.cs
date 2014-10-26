using LightInject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.DataAccess;

namespace Test.Framework.DataAccess
{
    public static class DbRegister
    {
        public static void Register(IList<string> connectionNames, SqlDbmsType dbmsType)
        {
            switch (dbmsType)
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

        private static void DbRegisterIoc(IList<string> connectionNames)
        {
            connectionNames.ToList().ForEach(connectionName =>
            {
                Container.RegisterInstance<IDatabase, Database>(
                    connectionName,
                    new Database(
                        Container.Resolve<IDbConnection>(connectionName), 
                        Container.Resolve<IOrm>(connectionName)),
                    ObjectLifeSpans.Singleton);
            });
        }

        private static void DbRegisterLightInject(IList<string> connectionNames)
        {
            connectionNames.ToList().ForEach(connectionName =>
            {
                var container = (ServiceContainer)Container.resolver.GetUnderlyingContainer();
                container.Register<IDatabase>(factory => 
                    new Database(
                        Container.Resolve<IDbConnection>(connectionName), 
                        Container.Resolve<IOrm>(connectionName)), 
                    connectionName, 
                    new PerContainerLifetime());
            });
        }
    }
}
