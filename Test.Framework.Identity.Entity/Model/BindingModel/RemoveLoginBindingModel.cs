﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity.Model
{
    public class RemoveLoginBindingModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
