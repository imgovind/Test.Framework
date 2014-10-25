using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class FrameworkSettings
    {
        private static IWebConfiguration configuration;

        static FrameworkSettings() 
        {
            if(configuration == null)
            {
                configuration = Container.Resolve<IWebConfiguration>();
            }
        }

        public static string ExtensibilityKey { get { return "{0}_Test_{1}_Key"; } }
        public static string NoReplyEmail { get { return configuration.AppSettings["NoReplyEmail"]; } }
        public static string CryptoSaltValue { get { return configuration.AppSettings["CryptoSaltValue"]; } }
        public static string CryptoPassPhrase { get { return configuration.AppSettings["CryptoPassPhrase"]; } }
        public static string CryptoInitVector { get { return configuration.AppSettings["CryptoInitVector"]; } }
        public static string TemplateDirectory { get { return configuration.AppSettings["TemplateDirectory"]; } }
        public static int CryptoKeySize { get { return configuration.AppSettings["CryptoKeySize"].ToInteger(); } }
        public static bool EnableSSLMail { get { return configuration.AppSettings["EnableSSLMail"].ToBoolean(); } }
        public static string NoReplyDisplayName { get { return configuration.AppSettings["NoReplyDisplayName"]; } }
        public static string CryptoHashAlgorithm { get { return configuration.AppSettings["CryptoHashAlgorithm"]; } }
        public static int CryptoPasswordIterations { get { return configuration.AppSettings["CryptoPasswordIterations"].ToInteger(); } }
        public static double CachingIntervalInMinutes { get { return configuration.AppSettings["CachingIntervalInMinutes"].ToDouble(); } }
        public static string MembaseSessionTimeoutInMinutes { get { return configuration.AppSettings["MembaseSessionTimeoutInMinutes"]; } }

        public static class ContainerKeys
        {
            public static string InProcCacherKey { get { return "Container_CacherKey_InProc"; } }
            public static string RedisCacherKey { get { return "Container_CacherKey_Redis"; } }
            public static string MemcacheCacherKey { get { return "Container_CacherKey_Memcache"; } }
            public static string OutProcCacherKey { get; set; }
        }
    }
}
