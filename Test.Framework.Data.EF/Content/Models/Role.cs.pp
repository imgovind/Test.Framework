using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $rootnamespace$.Models
{
    [Table("roles")]
    public class Role : IDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
