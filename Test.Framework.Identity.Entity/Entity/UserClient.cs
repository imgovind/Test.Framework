using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity.Entity
{
    public class UserClient
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ClientId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeprecated { get; set; }
        public DateTime Added { get; set; }
        public DateTime Revoked { get; set; }
    }
}
