using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using LightInject;

namespace Test.Framework.DataAccess
{
    public static class OrmRegister
    {
        public static void Register(IList<string> connectionNames, OrmType ormType, SqlDbmsType dbmsType)
        {
            switch (ormType)
            {
                case OrmType.Custom:
                    CustomRegister(connectionNames, dbmsType);
                    break;
                case OrmType.Dapper:
                    DapperRegisterLightInject(connectionNames, dbmsType);
                    break;
                default:
                    DapperRegisterLightInject(connectionNames, dbmsType);
                    break;
            }
        }

        private static void CustomRegister(IList<string> connectionNames, SqlDbmsType dbmsType)
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

        private static void DapperRegister(IList<string> connectionNames, SqlDbmsType dbmsType)
        {
            connectionNames.ToList().ForEach(connectionName =>
            {
                Container.RegisterInstance<IOrm, DapperOrm>(
                    connectionName,
                    new DapperOrm(connectionName, dbmsType),
                    ObjectLifeSpans.Singleton);
            });
        }

        private static void DapperRegisterLightInject(IList<string> connectionNames, SqlDbmsType dbmsType)
        {
            connectionNames.ToList().ForEach(connectionName =>
            {
                var container = (ServiceContainer)Container.resolver.GetUnderlyingContainer();
                container.Register<IOrm>(factory => 
                    new DapperOrm(connectionName, dbmsType), 
                    connectionName, 
                    new PerContainerLifetime());
            });
        }
    }
}
