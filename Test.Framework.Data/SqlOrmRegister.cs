using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using LightInject;

namespace Test.Framework.Data
{
    public static class SqlOrmRegister
    {
        public static void Register(IEnumerable<string> connectionNames, SqlDbmsType sqlDbType, OrmType ormType = OrmType.Dapper)
        {
            switch (ormType)
            {
                case OrmType.Custom:
                    CustomRegister(connectionNames, sqlDbType);
                    break;
                case OrmType.Dapper:
                    DapperRegisterLightInject(connectionNames, sqlDbType);
                    break;
                default:
                    DapperRegisterLightInject(connectionNames, sqlDbType);
                    break;
            }
        }

        private static void CustomRegister(IEnumerable<string> connectionNames, SqlDbmsType sqlDbType)
        {
            var configuration = Container.Resolve<IWebConfiguration>();
            connectionNames.ToList().ForEach(connectionName =>
            {
                Container.RegisterInstance<IOrm, CustomOrm>(
                    connectionName,
                    new CustomOrm(Container.Resolve<IDbConnection>(connectionName)),
                    ObjectLifeSpans.Singleton);
            });
        }

        private static void DapperRegister(IEnumerable<string> connectionNames, SqlDbmsType sqlDbType)
        {
            connectionNames.ToList().ForEach(connectionName =>
            {
                Container.RegisterInstance<IOrm, DapperOrm>(
                    connectionName,
                    new DapperOrm(connectionName, sqlDbType),
                    ObjectLifeSpans.Singleton);
            });
        }

        private static void DapperRegisterLightInject(IEnumerable<string> connectionNames, SqlDbmsType sqlDbType)
        {
            connectionNames.ToList().ForEach(connectionName =>
            {
                var container = (ServiceContainer)Container.IocContainer.GetUnderlyingContainer();
                container.Register<IOrm>(factory => 
                    new DapperOrm(connectionName, sqlDbType), 
                    connectionName, 
                    new PerContainerLifetime());
            });
        }
    }
}
