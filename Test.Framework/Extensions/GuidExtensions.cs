using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public static class GuidExtension
    {
        public static bool IsEmpty(this Guid target)
        {
            return target == Guid.Empty;
        }

        public static bool IsNotEmpty(this Guid target)
        {
            return target != Guid.Empty;
        }
    }
}
