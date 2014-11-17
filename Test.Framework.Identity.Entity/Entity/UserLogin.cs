using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity.Entity
{
    public class UserLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public Guid UserId { get; set; }
    }
}
