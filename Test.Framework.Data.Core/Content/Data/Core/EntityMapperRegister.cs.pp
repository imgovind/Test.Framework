using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Data;

namespace $rootnamespace$.Data
{
    public static class EntityMapperRegister
    {
        public static void Initialize()
        {
            EntityMapper.Register<TestModel>(new TestModelMapper());
        }
    }
}
