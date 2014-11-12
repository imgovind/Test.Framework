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
        public static void Register(IEnumerable<string> connectionNames, SqlDbmsType sqlDbmsType = SqlDbmsType.MySql, OrmType ormType = OrmType.Dapper)
        {
            switch (ormType)
            {
                case OrmType.Custom:
                    CustomRegister(connectionNames, sqlDbmsType);
                    break;
                case OrmType.Dapper:
                    DapperRegisterLightInject(connectionNames, sqlDbmsType);
                    break;
                default:
                    DapperRegisterLightInject(connectionNames, sqlDbmsType);
                    break;
            }
        }

        private static void CustomRegister(IEnumerable<string> connectionNames, SqlDbmsType sqlDbmsType)
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

        private static void DapperRegister(IEnumerable<string> connectionNames, SqlDbmsType sqlDbmsType)
        {
            connectionNames.ToList().ForEach(connectionName =>
            {
                Container.RegisterInstance<IOrm, DapperOrm>(
                    connectionName,
                    new DapperOrm(connectionName, sqlDbmsType),
                    ObjectLifeSpans.Singleton);
            });
        }

        private static void DapperRegisterLightInject(IEnumerable<string> connectionNames, SqlDbmsType sqlDbmsType)
        {
            connectionNames.ToList().ForEach(connectionName =>
            {
                var container = (ServiceContainer)Container.IocContainer.GetUnderlyingContainer();
                container.Register<IOrm>(factory => 
                    new DapperOrm(connectionName, sqlDbmsType), 
                    connectionName, 
                    new PerContainerLifetime());
            });
        }
    }
}
