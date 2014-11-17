using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Enum;

namespace Test.Framework.Identity.Entity
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public int ClientType { get; set; }
        public ClientTypes Type { get; set; }
        public bool IsActive { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }
        public bool IsDeprecated { get; set; }
    }
}
