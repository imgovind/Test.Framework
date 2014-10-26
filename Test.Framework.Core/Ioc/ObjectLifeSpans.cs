using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public enum ObjectLifeSpans
    {
        Singleton = 1,
        Transient = 2,
        WebRequest = 3,
        Thread = 4,
        Session = 5,
        Cached = 6
    }
}
