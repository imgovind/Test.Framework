using System;
using System.Data;

namespace Test.Framework.DataAccess
{
    public class Parameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public DbType Type { get; set; }
    }
}
