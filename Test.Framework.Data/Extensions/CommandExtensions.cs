using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Test.Framework.Data;

namespace Test.Framework.Extensions
{
    public static class CommandExtensions
    {
        [DebuggerStepThrough]
        public static void ClearCommand(this IDbCommand command, int timeout = 15)
        {
            command.CommandText = string.Empty;
            command.CommandTimeout = timeout;
            command.Parameters.Clear();
        }

        [DebuggerStepThrough]
        public static void AddParameter(this IDbCommand command, string parameterName, object parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.DbType = DbTypes.TypeMap[parameterValue.GetType()];
            command.Parameters.Add(parameter);
        }

        [DebuggerStepThrough]
        public static void AddParameter(this IDbCommand command, string parameterName, object parameterValue, DbType parameterType)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.DbType = parameterType;
            command.Parameters.Add(parameter);
        }

        [DebuggerStepThrough]
        public static void AddParameters(this IDbCommand command, IList<Parameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                command.AddParameter(parameter.Name, parameter.Value, parameter.Type);
            }
        }
    }
}
