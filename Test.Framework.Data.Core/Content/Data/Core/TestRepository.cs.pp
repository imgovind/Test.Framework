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

		public TestModel GetTestModel(int Id)
        {
            return this.Database.Select<TestModel>(x => x.TestIntProperty == Id).FirstOrDefault();
        }

        public bool AddTestModel(TestModel testModel)
        {
            return this.Database.Insert<TestModel>(testModel, false, "TestIntProperty");
        }

        public bool DeleteTestModel(TestModel testModel)
        {
            return this.Database.Delete<TestModel>(testModel, "TestIntProperty");
        }

        public bool UpdateTestModel(TestModel testModel)
        {
            return this.Database.Update<TestModel>(testModel, "TestIntProperty");
        }

        public bool DeprecateTestModel(TestModel testModel)
        {
            return this.Database.Deprecate<TestModel>(testModel, "TestIntProperty");
        }
    }
}
