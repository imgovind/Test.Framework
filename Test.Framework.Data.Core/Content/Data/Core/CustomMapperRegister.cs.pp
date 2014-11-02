using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $rootnamespace$.Data
{
    public static class CustomMapperRegister
    {
        public static void Initialize()
        {
            CustomMapper.Register<TestModel>(new TestModelMapper());
        }
    }
}
