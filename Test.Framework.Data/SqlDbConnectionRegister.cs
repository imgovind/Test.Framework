using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using LightInject;
using System.Data.SqlClient;

namespace Test.Framework.Data
{
    public static class SqlDbConnectionRegister
    {
        public static void Register(IEnumerable<string> connectionNames, SqlDbmsType sqlDbmsType = SqlDbmsType.MySql)
        {
            switch (sqlDbmsType)
            {
                case SqlDbmsType.MySql:
                    MySqlRegisterLightInject(connectionNames);
                    break;
                case SqlDbmsType.SqlServer:
                    SqlServerRegisterLightInject(connectionNames);
                    break;
                case SqlDbmsType.Oracle:
                    throw new NotImplementedException("Oracle Database Implementation Not Present");
                case SqlDbmsType.SqlLite:
                    throw new NotImplementedException("SqlLite Database Implementation Not Present");
                case SqlDbmsType.PostGreSql:
                    throw new NotImplementedException("PostGreSql Database Implementation Not Present");
                default:
                    MySqlRegisterLightInject(connectionNames);
                    break;
            }
        }

        private static void SqlServerRegisterLightInject(IEnumerable<string> connectionNames)
        {
            var configuration = Container.Resolve<IWebConfiguration>();
            connectionNames.ToList().ForEach(connectionName => {
                var container = (ServiceContainer)Container.IocContainer.GetUnderlyingContainer();
                container.Register<IDbConnection>(factory => 
                    new SqlConnection(configuration.ConnectionStrings(connectionName)), 
                    connectionName);
            });
        }

        private static void MySqlRegister(IEnumerable<string> connectionNames)
        {
            var configuration = Container.Resolve<IWebConfiguration>();
            connectionNames.ToList().ForEach(connectionName =>
            {
                Container.RegisterInstance<IDbConnection, MySqlConnection>(
                    connectionName, 
                    new MySqlConnection(configuration.ConnectionStrings(connectionName)), 
                    ObjectLifeSpans.Transient);
            });
        }
        private static void MySqlRegisterLightInject(IEnumerable<string> connectionNames)
        {
            var configuration = Container.Resolve<IWebConfiguration>();
            connectionNames.ToList().ForEach(connectionName =>
            {
                var container = (ServiceContainer)Container.IocContainer.GetUnderlyingContainer();
                container.Register<IDbConnection>(factory => 
                    new MySqlConnection(configuration.ConnectionStrings(connectionName)), 
                    connectionName);
            });
        }
    }
}
