using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Extensions;

namespace $rootnamespace$ 
{
    public partial class WebApiConstants
    {
        public static class WebApi
        {
            public static IWebConfiguration config { get { return Container.Resolve<IWebConfiguration>(); } }

            public static bool RequireHttps { get { var key = "webapi:RequireHttps"; return config.AppSettings[key].IsNotNullOrEmpty() ? config.AppSettings[key].ToBoolean() : false; } }
            public static bool CompressResponse { get { var key = "webapi:CompressResponse"; return config.AppSettings[key].IsNotNullOrEmpty() ? config.AppSettings[key].ToBoolean() : false; } }
            public static bool DecompressRequest { get { var key = "webapi:DecompressRequest"; return config.AppSettings[key].IsNotNullOrEmpty() ? config.AppSettings[key].ToBoolean() : false; } }
            public static bool EnrichResponse { get { var key = "webapi:EnrichResponse"; return config.AppSettings[key].IsNotNullOrEmpty() ? config.AppSettings[key].ToBoolean() : false; } }
            public static bool EnableVersioning { get { var key = "webapi:EnableVersioning"; return config.AppSettings[key].IsNotNullOrEmpty() ? config.AppSettings[key].ToBoolean() : false; } }
            public static bool EnableXmlFormatting { get { var key = "webapi:EnableXmlFormatting"; return config.AppSettings[key].IsNotNullOrEmpty() ? config.AppSettings[key].ToBoolean() : false; } }
            public static bool EnableJsonFormatting { get { var key = "webapi:EnableJsonFormatting"; return config.AppSettings[key].IsNotNullOrEmpty() ? config.AppSettings[key].ToBoolean() : false; } }
            public static bool EnableOutProcCaching { get { var key = "webapi:EnableOutProcCaching"; return config.AppSettings[key].IsNotNullOrEmpty() ? config.AppSettings[key].ToBoolean() : false; } }
            public static bool EnableInProcCaching { get { var key = "webapi:EnableInProcCaching"; return config.AppSettings[key].IsNotNullOrEmpty() ? config.AppSettings[key].ToBoolean() : false; } }
        }
    }
}
