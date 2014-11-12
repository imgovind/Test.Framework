using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $rootnamespace$.Models
{
    [Table("userlogins")]
    public class UserLogin : IDataModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
		[Key]
		public string UserId { get; set; }
    }
}
