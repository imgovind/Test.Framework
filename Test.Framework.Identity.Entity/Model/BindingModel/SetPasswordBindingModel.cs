using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity.Model
{
    public class SetPasswordBindingModel
    {
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
