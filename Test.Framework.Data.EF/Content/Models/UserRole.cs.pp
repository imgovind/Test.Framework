using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $rootnamespace$.Models
{
    [Table("userroles")]
    public class UserRole : IDataModel
    {
        [Key]
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
