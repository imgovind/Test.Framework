using System;
using System.Data;

namespace Test.Framework.Data
{
    public class Parameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public DbType Type { get; set; }
    }
}
