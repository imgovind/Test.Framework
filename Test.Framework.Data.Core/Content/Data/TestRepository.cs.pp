using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Data;

namespace $rootnamespace$.Data
{
    public class TestRepository : BaseRepository, ITestRepository
    {
        public TestRepository(IDatabase Database)
            : base(Database)
        {
        }

        public TestRepository(string connectionName)
            : base(connectionName)
        {
        }
    }
}
