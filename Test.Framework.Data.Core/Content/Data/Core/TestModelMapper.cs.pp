using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.Framework.Data;

namespace $rootnamespace$.Data
{
    public class TestModelMapper : ISelectable<TestModel>
    {
        public TestModel ApplySelect(DataReader reader)
        {
            return new TestModel 
            { 
                TestStringProperty = reader.GetStringNullable("TestStringPropertyColumnName"),
                TestGuidProperty = reader.GetGuid("TestGuidPropertyColumnName"),
                TestIntProperty = reader.GetInt("TestIntPropertyColumnName")
            };
        }

        public TestModel ApplySelect(DataReader reader, ISet<string> columns)
        {
            var result = new TestModel();

            if (columns.Contains("TestStringPropertyColumnName")) result.TestStringProperty = reader.GetStringNullable("TestStringPropertyColumnName");
            if (columns.Contains("TestIntPropertyColumnName")) result.TestIntProperty = reader.GetInt("TestIntPropertyColumnName");
            if (columns.Contains("TestGuidPropertyColumnName")) result.TestGuidProperty = reader.GetGuid("TestGuidPropertyColumnName");

            return result;
        }
    }
}
