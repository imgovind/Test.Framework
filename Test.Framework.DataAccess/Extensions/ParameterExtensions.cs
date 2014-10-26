using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Test.Framework.DataAccess;

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
    }
}
