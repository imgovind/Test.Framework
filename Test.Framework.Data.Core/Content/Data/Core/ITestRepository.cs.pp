using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $rootnamespace$.Data
{
    public interface ITestRepository
    {
		TestModel GetTestModel(int Id);
        bool AddTestModel(TestModel testModel);
        bool DeleteTestModel(TestModel testModel);
        bool UpdateTestModel(TestModel testModel);
        bool DeprecateTestModel(TestModel testModel);
    }
}
