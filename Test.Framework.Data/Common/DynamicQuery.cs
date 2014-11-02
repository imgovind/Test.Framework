using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Test.Framework.Extensions;

namespace Test.Framework.Data
{
    public static class DynamicQuery
    {
        #region Public Methods

        public static SqlDbCommand GetInsertQuery<T>(T item)
        {
            return GetInsertQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item);
        }

        public static SqlDbCommand GetInsertQuery<T>(string tableName, T item)
        {
            PropertyInfo[] props = PropertyCache.Get<T>();
            string[] columns = props.Select(p => p.Name).ToArray();

            var statement = string.Format("INSERT INTO {0} ({1}) VALUES (@{2})",
                                 tableName,
                                 string.Join(", ", columns),
                                 string.Join(", @", columns));

            IList<Parameter> parameters = new List<Parameter>();
            columns.ForEach(column =>
            {
                var prop = props.FirstOrDefault(p => p.Name.IsEqual(column));
                parameters.Add(new Parameter
                {
                    Name = column,
                    Value = prop.GetValue(item),
                    Type = DbTypes.TypeMap[prop.PropertyType]
                });
            });

            return new SqlDbCommand(statement, parameters);
        }

        public static SqlDbCommand GetUpdateQuery<T>(T item)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item);
        }

        public static SqlDbCommand GetUpdateQuery<T>(string tableName, T item)
        {
            PropertyInfo[] props = PropertyCache.Get<T>();
            string[] columns = props.Select(p => p.Name).ToArray();

            var parameterNames = columns.Select(name => name + "=@" + name.ToLower()).ToList();
            var statement = string.Format("UPDATE {0} SET {1} WHERE Id=@id", tableName, string.Join(", ", parameterNames));

            IList<Parameter> parameters = new List<Parameter>();
            columns.ForEach(column =>
            {
                var prop = props.FirstOrDefault(p => p.Name.IsEqual(column));
                parameters.Add(new Parameter { 
                    Name = column.ToLower(), 
                    Value = prop.GetValue(item),
                    Type = DbTypes.TypeMap[prop.PropertyType]
                });
            });

            return new SqlDbCommand(statement, parameters);
        }

        public static SqlDbCommand GetDeleteQuery<T>(T item)
        {
            return GetDeleteQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item);
        }

        public static SqlDbCommand GetDeleteQuery<T>(string tableName, T item)
        {
            PropertyInfo[] props = PropertyCache.Get<T>();
            string[] columns = props.Select(p => p.Name).ToArray();

            var parameterNames = columns.Select(name => name + "=@" + name.ToLower()).ToList();
            var statement = string.Format("DELETE FROM {0} WHERE {1}", tableName, string.Join(" AND ", parameterNames));

            IList<Parameter> parameters = new List<Parameter>();
            columns.ForEach(column => {
                var prop = props.FirstOrDefault(p => p.Name.IsEqual(column));
                parameters.Add(new Parameter { 
                    Name = column.ToLower(),
                    Value = prop.GetValue(item),
                    Type = DbTypes.TypeMap[prop.PropertyType]
                });
            });

            return new SqlDbCommand(statement, parameters);
        }

        public static SqlDbCommand GetDynamicQuery<T>(string tableName, Expression<Func<T, bool>> expression)
        {
            var builder = new StringBuilder();
            var queryProperties = new List<QueryParameter>();
            IList<Parameter> parameters = new List<Parameter>();

            var body = (BinaryExpression)expression.Body;
            WalkTree(body, ExpressionType.Default, ref queryProperties);

            builder.Append("SELECT * FROM ");
            builder.Append(tableName);
            builder.Append(" WHERE ");

            for (int i = 0; i < queryProperties.Count(); i++)
            {
                QueryParameter item = queryProperties[i];

                if (item.LinkingOperator.IsNotNullOrEmpty() && i > 0)
                {
                    builder.Append(string.Format("{0} {1} {2} @{1} ", item.LinkingOperator, item.PropertyName, item.QueryOperator, item.PropertyValue));
                }
                else
                {
                    builder.Append(string.Format("{0} {1} @{0} ", item.PropertyName, item.QueryOperator));
                }

                parameters.Add(new Parameter { Name = item.PropertyName, Value = item.PropertyValue, Type = item.DatabaseType });
            }

            return new SqlDbCommand(builder.ToString().TrimEnd(), parameters);
        }

        #endregion

        #region Private Methods

        private static void WalkTree(BinaryExpression body, ExpressionType linkingType, ref List<QueryParameter> queryProperties)
        {
            if (body.NodeType != ExpressionType.AndAlso && body.NodeType != ExpressionType.OrElse)
            {
                string link = GetOperator(linkingType);
                string propertyName = GetPropertyName(body);
                dynamic propertyValue = GetPropertyValue(body.Right);
                string databaseOperator = GetOperator(body.NodeType);
                DbType databaseType = DbTypes.TypeMap[propertyValue.GetType()];

                queryProperties.Add(new QueryParameter(link, propertyName, propertyValue, databaseOperator, databaseType));
            }
            else
            {
                WalkTree((BinaryExpression)body.Left, body.NodeType, ref queryProperties);
                WalkTree((BinaryExpression)body.Right, body.NodeType, ref queryProperties);
            }
        }

        private static string GetPropertyName(BinaryExpression body)
        {
            string propertyName = body.Left.ToString().Split(new char[] { '.' })[1];

            if (body.Left.NodeType == ExpressionType.Convert)
            {
                propertyName = propertyName.Replace(")", string.Empty);
            }

            return propertyName;
        }

        private static string GetOperator(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "!=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    return "AND";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return "OR";
                case ExpressionType.Default:
                    return string.Empty;
                default:
                    throw new NotImplementedException();
            }
        }

        private static object GetPropertyValue(Expression body)
        {
            var objectMember = Expression.Convert(body, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        #endregion
    }
}
