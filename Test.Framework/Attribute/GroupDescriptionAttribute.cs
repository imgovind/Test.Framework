using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class GroupDescriptionAttribute : DescriptionAttribute
    {
        public GroupDescriptionAttribute(string description, string group)
            : base(description)
        {
            this.Group = group;
        }

        public string Group { get; private set; }
    }
}
