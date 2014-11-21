using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Data.Extensions;

namespace Test.Framework.Identity.Data
{
    public sealed class ShardLookup
    {
        private static readonly Lazy<ShardLookup> lazy =
            new Lazy<ShardLookup>(() => new ShardLookup());

        public static ShardLookup Instance { get { return lazy.Value; } }

        private ShardLookup()
        {
        }

        public sealed class Login
        {
            private static readonly Lazy<Login> lazy =
                new Lazy<Login>(() => new Login());

            public static Login Instance { get { return lazy.Value; } }

            private Login()
            {
            }

            public LoginShardConfigurationSection LoginShardConfig = ConfigurationManager.GetSection("loginShardLookup") as LoginShardConfigurationSection;

            #region Private Members
            private Dictionary<int, string> _DbStore = new Dictionary<int, string>() {
                {0, "mysql:AuthenticationConnectionString"}
            };

            private Dictionary<string, int> _LoginShardStore = new Dictionary<string, int>() { 

                {"a", 0},
                {"A", 0},

                {"b", 0},
                {"B", 0},

                {"c", 0},
                {"C", 0},

                {"d", 0},
                {"D", 0},

                {"e", 0},
                {"E", 0},

                {"f", 0},
                {"F", 0},

                {"g", 0},
                {"G", 0},

                {"h", 0},
                {"H", 0},

                {"i", 0},
                {"I", 0},

                {"j", 0},
                {"J", 0},

                {"k", 0},
                {"K", 0},

                {"l", 0},
                {"L", 0},

                {"m", 0},
                {"M", 0},

                {"n", 0},
                {"N", 0},

                {"o", 0},
                {"O", 0},

                {"p", 0},
                {"P", 0},

                {"q", 0},
                {"Q", 0},

                {"r", 0},
                {"R", 0},

                {"s", 0},
                {"S", 0},

                {"t", 0},
                {"T", 0},

                {"u", 0},
                {"U", 0},

                {"v", 0},
                {"V", 0},

                {"w", 0},
                {"W", 0},

                {"x", 0},
                {"X", 0},

                {"y", 0},
                {"Y", 0},

                {"z", 0},
                {"Z", 0},
            };
            #endregion

            public Dictionary<string, int> LoginShardStore 
            { 
                get 
                { 
                    if(LoginShardConfig == null)
                        return _LoginShardStore;

                    return LoginShardConfig.LoginShards.ToDictionary();
                }
            }

            public Dictionary<int, string> DbShardStore
            {
                get
                {
                    if (LoginShardConfig == null)
                        return _DbStore;

                    return LoginShardConfig.DbShards.ToDictionary();
                }
            }
        }
    }
}
