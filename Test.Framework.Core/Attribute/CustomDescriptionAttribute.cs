using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class CustomDescriptionAttribute : DescriptionAttribute
    {
        public CustomDescriptionAttribute(string description, string shortDescription)
            : base(description)
        {
            this.ShortDescription = shortDescription;
        }

        public CustomDescriptionAttribute(string description, string shortDescription, string category)
            : base(description)
        {
            this.Category = category;
            this.ShortDescription = shortDescription;
        }

        public CustomDescriptionAttribute(string description, string shortDescription, string category, string statusGroup)
            : base(description)
        {
            this.Category = category;
            this.StatusGroup = statusGroup;
            this.ShortDescription = shortDescription;
        }

        public CustomDescriptionAttribute(string description, string shortDescription, string category, string statusGroup, string formGroup)
            : base(description)
        {
            this.Category = category;
            this.StatusGroup = statusGroup;
            this.ShortDescription = shortDescription;
            this.FormGroup = formGroup;
        }

        public CustomDescriptionAttribute(string description, string shortDescription, string category, string statusGroup, string formGroup, string actualDescription)
            : base(description)
        {
            this.Category = category;
            this.StatusGroup = statusGroup;
            this.ShortDescription = shortDescription;
            this.FormGroup = formGroup;
            this.ActualDescription = actualDescription;
        }


        public string Category { get; private set; }
        public string StatusGroup { get; private set; }
        public string ShortDescription { get; private set; }
        public string FormGroup { get; private set; }
        public string ActualDescription { get; private set; }
    }
}
