using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.DataAccess;
using Dapper;
using System.Diagnostics;

namespace Test.Framework.Extensions
{
    public static class ConnectionExtensions
    {
        public static IDbConnection OpenConnection(this IDbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();
            return dbConnection;
        }

        public static IDbCommand CreateCustomCommand(this IDbConnection connection, string sql, IEnumerable<Parameter> parameters = null, int timeOut = 15)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            if(parameters != null) command.AddParameters(parameters.ToList());
            command.CommandTimeout = timeOut;
            return command;
        }

        public static IDbCommand CreateCustomCommand(this IDbConnection connection, SqlCommand query)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query.Statement;
            command.AddParameters(query.Parameters);
            command.CommandTimeout = query.Timeout;
            return command;
        }

        public static CommandDefinition CreateDapperCommand(this IDbConnection connection, string statement, IEnumerable<Parameter> parameters = null, int timeout = 15)
        {
            string commandText = statement;
            object commandParameters = null;
            if (parameters != null) commandParameters = (object)parameters.GetDapperParameters();
            IDbTransaction transaction = null;
            int? commandTimeout = timeout;
            CommandType? commandType = null;
            var command = new CommandDefinition(commandText, commandParameters, transaction, commandTimeout, commandType, CommandFlags.None);
            return command;
        }

        public static CommandDefinition CreateDapperCommandAsync(this IDbConnection connection, string statement, IDbTransaction transaction = null, IEnumerable<Parameter> parameters = null, int timeout = 15)
        {
            string commandText = statement;
            object commandParameters = null;
            if (parameters != null) commandParameters = (object)parameters.GetDapperParameters();
            int? commandTimeout = timeout;
            CommandType? commandType = null;
            var command = new CommandDefinition(commandText, commandParameters, transaction, commandTimeout, commandType, CommandFlags.Buffered);
            return command;
        }

        public static CommandDefinition CreateDapperCommand(this IDbConnection connection, SqlCommand query, IDbTransaction transaction = null)
        {
            string commandText = query.Statement;
            object commandParameters = null;
            if (query.Parameters.IsNotNullOrEmpty())
            {
                commandParameters = (object)query.Parameters.GetDapperParameters();
            }
            int? commandTimeout = query.Timeout;
            CommandType? commandType = null;
            var command = new CommandDefinition(commandText, commandParameters, transaction, commandTimeout, commandType, CommandFlags.None);
            return command;
        }

        public static CommandDefinition CreateDapperCommandAsync(this IDbConnection connection, SqlCommand query, IDbTransaction transaction = null)
        {
            string commandText = query.Statement;
            object commandParameters = null;
            if (query.Parameters.IsNotNullOrEmpty())
            {
                commandParameters = (object)query.Parameters.GetDapperParameters();
            }
            int? commandTimeout = query.Timeout;
            CommandType? commandType = null;
            var command = new CommandDefinition(commandText, commandParameters, transaction, commandTimeout, commandType, CommandFlags.Buffered);
            return command;
        }

    }
}
