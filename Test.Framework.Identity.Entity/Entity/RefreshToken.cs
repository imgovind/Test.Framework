using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity.Entity
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public Guid UserClientId { get; set; }
        public string Subject { get; set; }
        public string ProtectedTicket { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiredUtc { get; set; }
    }
}
