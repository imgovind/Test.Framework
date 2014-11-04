using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Test.Framework.Data;

namespace Test.Framework.Extensions
{
    public static class ParameterExtensions
    {
        public static void AddParameter(this IList<Parameter> parameters, string parameterName, object parameterValue)
        {
            if (parameters != null)
            {
                parameters.Add(new Parameter() { Name = parameterName, Value = parameterValue, Type = DbTypes.TypeMap[parameterValue.GetType()] });
            }
        }

        public static dynamic GetDapperParameters(this IEnumerable<Parameter> parameters)
        {
            var param = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                param.Add(parameter.Name, parameter.Value);
            }
            return param;
        }

        public static object[] ToObjectArray(this IList<Parameter> parameters, bool ForValueOnly = false)
        {
            if(!ForValueOnly)
                return parameters.Cast<object>().ToArray();
            return parameters.Select(x => x.Value).Cast<object>().ToArray();
        }

        public static string ToPetaPocoValues(this IList<string> columns, bool includeColumnName = false, int parameterOffset = 0)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < columns.Count; i++)
            {
                parameterOffset += i;
                if (!includeColumnName)
                    result.Add("@" + parameterOffset.ToString());
                else
                    result.Add(columns[i] + " = @" + parameterOffset.ToString());
            }
            return string.Join(", ", result);
        }
    }
}
