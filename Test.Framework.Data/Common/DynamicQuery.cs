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

        #region Insert Query

        public static SqlDbCommand GetInsertQuery<T>(T item)
        {
            return GetInsertQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, false, null, false);
        }

        public static SqlDbCommand GetInsertQuery<T>(T item, bool IsQueryForPetaPoco)
        {
            return GetInsertQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, false, null, IsQueryForPetaPoco);
        }

        public static SqlDbCommand GetInsertQuery<T>(T item, bool IsAutoIncrement = false, string primaryKeyColumn = "Id",  bool IsQueryForPetaPoco = false)
        {
            return GetInsertQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, false, primaryKeyColumn, IsQueryForPetaPoco);
        }

        public static SqlDbCommand GetInsertQuery<T>(string tableName, T item, bool IsAutoIncrement = false, string primaryKeyColumn = "Id", bool IsQueryForPetaPoco = false)
        {
            List<string> builder = new List<string>();
            PropertyInfo[] props = PropertyCache.Resolve<T>();

            if (primaryKeyColumn.IsNotNullOrEmpty() && IsAutoIncrement)
                props = props.Where(x => !x.Name.Equals(primaryKeyColumn.ToLowerInvariant())).ToArray<PropertyInfo>();

            string[] columns = props.Select(p => p.Name).ToArray();

            if (tableName.IsNullOrEmpty())
                tableName = typeof(T).Name.ToLowerInvariant().Pluralize();

            builder.Add("INSERT INTO");
            builder.Add(tableName);
            builder.Add("(");
            builder.Add(string.Join(", ", columns));
            builder.Add(")");
            builder.Add("VALUES (@");
            if (!IsQueryForPetaPoco)
                builder.Add(string.Join(", @", columns));
            else
                builder.Add(columns.ToPetaPocoValues());
            builder.Add(")");

            //var statement = string.Format("INSERT INTO {0} ({1}) VALUES (@{2})",
                                 //tableName,
                                 //string.Join(", ", columns),
                                 //string.Join(", @", columns));

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

            return new SqlDbCommand(string.Join(" ", builder), parameters);
        }
        
        #endregion

        #region Update Query

        public static SqlDbCommand GetUpdateQuery<T>(T item, bool IsQueryForPetaPoco = false)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, IsQueryForPetaPoco);
        }

        public static SqlDbCommand GetUpdateQuery<T>(T item, string primaryKeyColumn = null,  bool IsQueryForPetaPoco = false)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, IsQueryForPetaPoco, primaryKeyColumn);
        }

        public static SqlDbCommand GetUpdateQuery<T>(T item, Expression<Func<T, bool>> expression, string primaryKeyColumn = null, bool IsQueryForPetaPoco = false)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, IsQueryForPetaPoco, primaryKeyColumn, expression);
        }

        public static SqlDbCommand GetUpdateQuery<T>(string tableName, T item, bool IsQueryForPetaPoco = false, string primaryKeyColumn = "Id", Expression<Func<T, bool>> expression = null, string defaultPrimaryColumn = "Id")
        {
            List<string> builder = new List<string>();
            PropertyInfo[] props = PropertyCache.Resolve<T>();

            PropertyInfo primaryProp = null;

            if (primaryKeyColumn.IsNotNullOrEmpty())
                primaryProp = props.Where(x => x.Name.ToLowerInvariant().Equals(primaryKeyColumn.ToLowerInvariant())).FirstOrDefault();
            else
                primaryProp = props.Where(x => x.Name.ToLowerInvariant().Equals(defaultPrimaryColumn.ToLowerInvariant())).FirstOrDefault();

            string[] columns = props.Where(x => !x.Name.Equals(primaryProp.Name)).Select(p => p.Name).ToArray();
            var parameterNames = columns.Select(name => name + "=@" + name.ToLower()).ToList();

            if (tableName.IsNullOrEmpty())
                tableName = typeof(T).Name.ToLowerInvariant().Pluralize();

            builder.Add("UPDATE");
            builder.Add(tableName);
            builder.Add("SET");
            if (IsQueryForPetaPoco)
                builder.Add(columns.ToPetaPocoValues(true));
            else
                builder.Add(string.Join(", ",parameterNames));

            IList<Parameter> parameters = new List<Parameter>();
            columns.ForEach(column =>
            {
                var prop = props.FirstOrDefault(p => p.Name.IsEqual(column));
                parameters.Add(new Parameter
                {
                    Name = column.ToLower(),
                    Value = prop.GetValue(item),
                    Type = DbTypes.TypeMap[prop.PropertyType]
                });
            });

            if (expression != null)
                return ExpressionTreeWalker<T>(expression, builder, parameters, IsQueryForPetaPoco);

            builder.Add("WHERE");
            builder.Add(new List<string> { primaryKeyColumn }.ToPetaPocoValues(true, parameters.Count));
            parameters.Add(new Parameter { Name = primaryKeyColumn.ToLower(), Value = primaryProp.GetValue(item), Type = DbTypes.TypeMap[primaryProp.PropertyType]});
            return new SqlDbCommand(string.Join(" ", builder), parameters);
        }

        //public static SqlDbCommand GetUpdateQuery<T>(string tableName, T item, int a, string primaryKeyColumn = null)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    PropertyInfo[] props = PropertyCache.Resolve<T>();
        //    builder.Append("UPDATE ");

        //    string[] columns = props.Select(p => p.Name).ToArray();

        //    var parameterNames = columns.Select(name => name + "=@" + name.ToLower()).ToList();
        //    var statement = string.Format("UPDATE {0} SET {1} WHERE Id=@id", tableName, string.Join(", ", parameterNames));

        //    IList<Parameter> parameters = new List<Parameter>();
        //    columns.ForEach(column =>
        //    {
        //        var prop = props.FirstOrDefault(p => p.Name.IsEqual(column));
        //        parameters.Add(new Parameter
        //        {
        //            Name = column.ToLower(),
        //            Value = prop.GetValue(item),
        //            Type = DbTypes.TypeMap[prop.PropertyType]
        //        });
        //    });

        //    return new SqlDbCommand(statement, parameters);
        //}
        
        #endregion

        #region Delete Query

        public static SqlDbCommand GetDeleteQuery<T>(T item)
        {
            return GetDeleteQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item);
        }

        public static SqlDbCommand GetDeleteQuery<T>(string tableName, T item)
        {
            PropertyInfo[] props = PropertyCache.Resolve<T>();

            if (props == null)
                PropertyCache.Register<T>();

            props = PropertyCache.Resolve<T>();

            string[] columns = props.Select(p => p.Name).ToArray();

            if (tableName.IsNullOrEmpty())
                tableName = typeof(T).Name.ToLowerInvariant().Pluralize();

            var parameterNames = columns.Select(name => name + "=@" + name.ToLower()).ToList();
            var statement = string.Format("DELETE FROM {0} WHERE {1}", tableName, string.Join(" AND ", parameterNames));

            IList<Parameter> parameters = new List<Parameter>();
            columns.ForEach(column =>
            {
                var prop = props.FirstOrDefault(p => p.Name.IsEqual(column));
                parameters.Add(new Parameter
                {
                    Name = column.ToLower(),
                    Value = prop.GetValue(item),
                    Type = DbTypes.TypeMap[prop.PropertyType]
                });
            });

            return new SqlDbCommand(statement, parameters);
        }
        
        #endregion

        #region Select Query

        public static SqlDbCommand GetDynamicQuery<T>(Expression<Func<T, bool>> expression = null, bool IsQueryForPetaPoco = false)
        {
            var tableName = typeof(T).Name.ToLowerInvariant().Pluralize();
            return GetDynamicQuery<T>(tableName, expression, IsQueryForPetaPoco);
        }

        public static SqlDbCommand GetDynamicQuery<T>(string tableName, Expression<Func<T, bool>> expression = null, bool IsQueryForPetaPoco = false)
        {
            var builder = new List<string>();
            IList<Parameter> parameters = new List<Parameter>();

            if (tableName.IsNullOrEmpty())
                tableName = typeof(T).Name.ToLowerInvariant().Pluralize();

            builder.Add("SELECT * FROM");
            builder.Add(tableName);

            if (expression == null)
                return new SqlDbCommand(string.Join(" ", builder).TrimEnd(), parameters);

            return ExpressionTreeWalker<T>(expression, builder, parameters, IsQueryForPetaPoco);
        }

        private static SqlDbCommand ExpressionTreeWalker<T>(Expression<Func<T, bool>> expression, List<string> builder, IList<Parameter> parameters, bool IsQueryForPetaPoco = false)
        {
            builder.Add("WHERE");

            var body = (BinaryExpression)expression.Body;
            var queryProperties = new List<QueryParameter>();
            WalkTree(body, ExpressionType.Default, ref queryProperties);

            var parameterOffset = parameters.Count;
            for (int i = 0; i < queryProperties.Count(); i++)
            {
                parameterOffset += i;
                QueryParameter item = queryProperties[i];

                if (item.LinkingOperator.IsNotNullOrEmpty() && i > 0)
                {
                    if (!IsQueryForPetaPoco)
                        builder.Add(item.LinkingOperator + " " + item.PropertyName + " " + item.QueryOperator + " @" + item.PropertyName);
                        //builder.Add(string.Format("{0} {1} {2} @{1}", item.LinkingOperator, item.PropertyName, item.QueryOperator, item.PropertyValue));
                    else
                        builder.Add(item.LinkingOperator + " " + item.PropertyName + " " + item.QueryOperator + " @" + parameterOffset.ToString());
                        //builder.Add(string.Format("{0} {1} {2} @{4}", item.LinkingOperator, item.PropertyName, item.QueryOperator, item.PropertyValue, i));
                }
                else
                {
                    if (!IsQueryForPetaPoco)
                        builder.Add(item.PropertyName + " " + item.QueryOperator + " @" + item.PropertyName);
                        //builder.Add(string.Format("{0} {1} @{0}", item.PropertyName, item.QueryOperator));
                    else
                        builder.Add(item.PropertyName + " " + item.QueryOperator + " @" + parameterOffset.ToString());
                        //builder.Add(string.Format("{0} {1} @{2}", item.PropertyName, item.QueryOperator, i));
                }

                parameters.Add(new Parameter { Name = item.PropertyName, Value = item.PropertyValue, Type = item.DatabaseType });
            }

            return new SqlDbCommand(string.Join(" ", builder).TrimEnd(), parameters);
        }

        #endregion

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
