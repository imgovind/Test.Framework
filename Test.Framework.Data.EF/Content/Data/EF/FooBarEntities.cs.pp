using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using $rootnamespace$.Models.EF;

namespace $rootnamespace$.Data.EF
{
    public class FooBarEntities : DbContext
    {
        public FooBarEntities(string connectionName)
            : base(connectionName)
        {
        }

        public DbSet<Bar> Bars { get; set; }
        public DbSet<Foo> Foos { get; set; }
    }
}
