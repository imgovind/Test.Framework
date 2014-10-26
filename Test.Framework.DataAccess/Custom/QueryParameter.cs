using System;
using System.Data;

namespace Test.Framework.DataAccess
{
    internal class QueryParameter
    {
        public string PropertyName { get; set; }
        public DbType DatabaseType { get; set; }
        public object PropertyValue { get; set; }
        public string QueryOperator { get; set; }
        public string LinkingOperator { get; set; }

        internal QueryParameter(string linkingOperator, string propertyName, object propertyValue, string queryOperator, DbType databaseType)
        {
            this.PropertyName = propertyName;
            this.DatabaseType = databaseType;
            this.PropertyValue = propertyValue;
            this.QueryOperator = queryOperator;
            this.LinkingOperator = linkingOperator;
        }

    }
}
