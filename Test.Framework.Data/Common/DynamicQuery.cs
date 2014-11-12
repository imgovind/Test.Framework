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

        public static DQueryBase<T> Insert<T>(T item)
        {
            return new DQueryInsert<T>(item);
        }

        public static SqlDbCommand GetInsertQuery<T>(T item, OrmType ormType = OrmType.Dapper)
        {
            return GetInsertQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, false, null, 15, ormType);
        }

        public static SqlDbCommand GetInsertQuery<T>(T item, int timeout, OrmType ormType = OrmType.Dapper)
        {
            return GetInsertQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, false, null, timeout, ormType);
        }

        public static SqlDbCommand GetInsertQuery<T>(T item, bool IsAutoIncrement, int timeout = 15, OrmType ormType = OrmType.Dapper)
        {
            return GetInsertQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, IsAutoIncrement, null, timeout, ormType);
        }

        public static SqlDbCommand GetInsertQuery<T>(T item, bool IsAutoIncrement, string primaryKeyColumn = "Id", int timeout = 15, OrmType ormType = OrmType.Dapper)
        {
            return GetInsertQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, IsAutoIncrement, primaryKeyColumn, timeout, ormType);
        }

        public static SqlDbCommand GetInsertQuery<T>(string tableName, T item, bool IsAutoIncrement = false, string primaryKeyColumn = "Id", int timeout = 15, OrmType ormType = OrmType.Dapper, string defaultPrimaryColumn = "Id")
        {
            List<string> builder = new List<string>();
            PropertyInfo[] props = PropertyCache.Resolve<T>();

            if ((primaryKeyColumn.IsNotNullOrEmpty()) && IsAutoIncrement)
                props = props.Where(x => !x.Name.Equals(primaryKeyColumn)).ToArray<PropertyInfo>();
            else if(IsAutoIncrement)
                props = props.Where(x => !x.Name.Equals(defaultPrimaryColumn)).ToArray<PropertyInfo>();

            string[] columns = props.Select(p => p.Name).ToArray();

            if (tableName.IsNullOrEmpty())
                tableName = typeof(T).Name.ToLowerInvariant().Pluralize();

            builder.Add("INSERT INTO");
            builder.Add(tableName);
            builder.Add("(");
            builder.Add(string.Join(", ", columns));
            builder.Add(")");
            builder.Add("VALUES (");
            switch (ormType)
            {
                case OrmType.Custom:
                case OrmType.Dapper:
                    builder.Add("@"+string.Join(", @", columns));
                    break;
                case OrmType.PetaPoco:
                    builder.Add(columns.ToPetaPocoValues());
                    break;
                case OrmType.SubSonic:
                    builder.Add(columns.ToSubSonicValues());
                    break;
                case OrmType.EntityFramework:
                default:
                    return null;
            }
            builder.Add(")");

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

            return new SqlDbCommand(string.Join(" ", builder), parameters, timeout);
        }
        
        #endregion

        #region Select Query

        public static DQueryBase<T> Select<T>()
        {
            return new DQuerySelect<T>(default(T));
        }

        public static SqlDbCommand GetSelectQuery<T>(Expression<Func<T, bool>> expression = null, OrmType ormType = OrmType.Dapper)
        {
            return GetSelectQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), expression, 15, ormType);
        }

        public static SqlDbCommand GetSelectQuery<T>(int timeout, Expression<Func<T, bool>> expression = null, OrmType ormType = OrmType.Dapper)
        {
            return GetSelectQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), expression, timeout, ormType);
        }
        
        public static SqlDbCommand GetSelectQuery<T>(string tableName, Expression<Func<T, bool>> expression = null, int timeout = 15, OrmType ormType = OrmType.Dapper)
        {
            var builder = new List<string>();
            IList<Parameter> parameters = new List<Parameter>();

            if (tableName.IsNullOrEmpty())
                tableName = typeof(T).Name.ToLowerInvariant().Pluralize();

            builder.Add("SELECT * FROM");
            builder.Add(tableName);

            if (expression == null)
                return new SqlDbCommand(string.Join(" ", builder).TrimEnd(), parameters, timeout);

            return ExpressionTreeWalker<T>(expression, builder, parameters, ormType, timeout);
        }

        public static SqlDbCommand GetPageQuery<T>(int take = 50, int skip = 0, int timeout = 15, OrmType ormType = OrmType.Dapper)
        {
            return GetPageQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), null, take, skip, timeout, ormType);
        }

        public static SqlDbCommand GetPageQuery<T>(Expression<Func<T, bool>> expression = null, int take = 50, int skip = 0, int timeout = 15, OrmType ormType = OrmType.Dapper)
        {
            return GetPageQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), expression, take, skip, timeout, ormType);
        }

        public static SqlDbCommand GetPageQuery<T>(string tableName, Expression<Func<T, bool>> expression = null, int take = 50, int skip = 0, int timeout = 15, OrmType ormType = OrmType.Dapper)
        {
            var builder = new List<string>();
            IList<Parameter> parameters = new List<Parameter>();

            if (tableName.IsNullOrEmpty())
                tableName = typeof(T).Name.ToLowerInvariant().Pluralize();

            builder.Add("SELECT * FROM");
            builder.Add(tableName);

            if (expression == null)
            {
                builder.Add("LIMIT " + take.ToString());
                if (skip != 0)
                    builder.Add("OFFSET " + skip.ToString());
                return new SqlDbCommand(string.Join(" ", builder).TrimEnd(), parameters, timeout);
            }

            return ExpressionTreeWalker<T>(expression, builder, parameters, ormType, timeout, take, skip);
        }

        #endregion

        #region Update Query

        public static DQueryBase<T> Update<T>(T item)
        {
            return new DQueryUpdate<T>(item);
        }

        public static SqlDbCommand GetUpdateQuery<T>(T item, HashSet<string> excludedColumns = null, OrmType ormType = OrmType.Dapper)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, null, null, 15, excludedColumns);
        }

        public static SqlDbCommand GetUpdateQuery<T>(T item, int timeout, HashSet<string> excludedColumns = null, OrmType ormType = OrmType.Dapper)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, null, null, timeout, excludedColumns);
        }

        public static SqlDbCommand GetUpdateQuery<T>(T item, string primaryKeyColumn, HashSet<string> excludedColumns = null, OrmType ormType = OrmType.Dapper)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, primaryKeyColumn, null, 15, excludedColumns);
        }

        public static SqlDbCommand GetUpdateQuery<T>(T item, string primaryKeyColumn, int timeout, HashSet<string> excludedColumns = null, OrmType ormType = OrmType.Dapper)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, primaryKeyColumn, null, timeout, excludedColumns);
        }

        public static SqlDbCommand GetUpdateQuery<T>(T item, Expression<Func<T, bool>> expression, HashSet<string> excludedColumns = null, OrmType ormType = OrmType.Dapper)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, null, expression, 15, excludedColumns);
        }

        public static SqlDbCommand GetUpdateQuery<T>(T item, Expression<Func<T, bool>> expression, int timeout, HashSet<string> excludedColumns = null, OrmType ormType = OrmType.Dapper)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, null, expression, timeout, excludedColumns);
        }

        public static SqlDbCommand GetUpdateQuery<T>(T item, Expression<Func<T, bool>> expression, string primaryKeyColumn, HashSet<string> excludedColumns = null, OrmType ormType = OrmType.Dapper)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, primaryKeyColumn, expression, 15, excludedColumns);
        }

        public static SqlDbCommand GetUpdateQuery<T>(T item, Expression<Func<T, bool>> expression, string primaryKeyColumn, int timeout, HashSet<string> excludedColumns = null, OrmType ormType = OrmType.Dapper)
        {
            return GetUpdateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, primaryKeyColumn, expression, timeout, excludedColumns);
        }

        public static SqlDbCommand GetUpdateQuery<T>(string tableName, T item, OrmType ormType = OrmType.Dapper, string primaryKeyColumn = "Id", Expression<Func<T, bool>> expression = null, int timeout = 15, HashSet<string> excludedColumns = null, string defaultPrimaryColumn = "Id")
        {
            List<string> builder = new List<string>();
            PropertyInfo[] props = PropertyCache.Resolve<T>();

            PropertyInfo primaryProp = null;

            if (primaryKeyColumn.IsNotNullOrEmpty())
                primaryProp = props.Where(x => x.Name.Equals(primaryKeyColumn)).FirstOrDefault();
            else
                primaryProp = props.Where(x => x.Name.Equals(defaultPrimaryColumn)).FirstOrDefault();

            props = props.Where(x => !x.Name.Equals(primaryProp.Name)).ToArray();

            if (excludedColumns.IsNotNullOrEmpty())
                props = props.Where(x => !excludedColumns.Contains(x.Name)).ToArray();

            string[] columns = props.Select(p => p.Name).ToArray();
            var parameterNames = columns.Select(name => name + "=@" + name.ToLowerInvariant()).ToList();

            if (tableName.IsNullOrEmpty())
                tableName = typeof(T).Name.ToLowerInvariant().Pluralize();

            builder.Add("UPDATE");
            builder.Add(tableName);
            builder.Add("SET");
            switch (ormType)
            {
                case OrmType.Custom:
                case OrmType.Dapper:
                    builder.Add(string.Join(", ",parameterNames));
                    break;
                case OrmType.PetaPoco:
                    builder.Add(columns.ToPetaPocoValues(true));
                    break;
                case OrmType.SubSonic:
                    builder.Add(columns.ToSubSonicValues(true));
                    break;
                case OrmType.EntityFramework:
                default:
                    return null;
            }

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
                return ExpressionTreeWalker<T>(expression, builder, parameters, ormType, timeout);

            builder.Add("WHERE");
            switch (ormType)
            {
                case OrmType.Custom: 
                case OrmType.Dapper:
                    builder.Add(primaryProp.Name + "=@" + primaryProp.Name);
                    break;
                case OrmType.PetaPoco:
                    builder.Add(new List<string> { primaryProp.Name }.ToPetaPocoValues(true, parameters.Count));
                    break;
                case OrmType.SubSonic:
                    builder.Add(new List<string> { primaryProp.Name }.ToSubSonicValues(true, parameters.Count));
                    break;
                case OrmType.EntityFramework:
                default:
                    return null;
            }
            parameters.Add(new Parameter { Name = primaryProp.Name, Value = primaryProp.GetValue(item), Type = DbTypes.TypeMap[primaryProp.PropertyType]});
            return new SqlDbCommand(string.Join(" ", builder), parameters, timeout);
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

        public static DQueryBase<T> Delete<T>()
        {
            return new DQueryDelete<T>(default(T));
        }

        public static SqlDbCommand GetDeleteQuery<T>(T item, OrmType ormType = OrmType.Dapper)
        {
            return GetDeleteQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType);
        }

        public static SqlDbCommand GetDeleteQuery<T>(T item, int timeout, OrmType ormType = OrmType.Dapper)
        {
            return GetDeleteQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, null, null, timeout);
        }

        public static SqlDbCommand GetDeleteQuery<T>(T item, string primaryKeyColumn, OrmType ormType = OrmType.Dapper)
        {
            return GetDeleteQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, primaryKeyColumn);
        }

        public static SqlDbCommand GetDeleteQuery<T>(T item, string primaryKeyColumn, int timeout, OrmType ormType = OrmType.Dapper)
        {
            return GetDeleteQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, primaryKeyColumn, null, timeout);
        }

        public static SqlDbCommand GetDeleteQuery<T>(Expression<Func<T, bool>> expression, OrmType ormType = OrmType.Dapper)
        {
            return GetDeleteQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), default(T), ormType, null, expression);
        }

        public static SqlDbCommand GetDeleteQuery<T>(Expression<Func<T, bool>> expression, int timeout, OrmType ormType = OrmType.Dapper)
        {
            return GetDeleteQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), default(T), ormType, null, expression, timeout);
        }

        public static SqlDbCommand GetDeleteQuery<T>(string tableName, T item, OrmType ormType = OrmType.Dapper, string primaryKeyColumn = "Id", Expression<Func<T, bool>> expression = null, int timeout = 15, string defaultPrimaryColumn = "Id")
        {
            List<string> builder = new List<string>();
            PropertyInfo[] props = PropertyCache.Resolve<T>();

            PropertyInfo primaryProp = null;

            if (primaryKeyColumn.IsNotNullOrEmpty())
                primaryProp = props.Where(x => x.Name.Equals(primaryKeyColumn)).FirstOrDefault();
            else
                primaryProp = props.Where(x => x.Name.Equals(defaultPrimaryColumn)).FirstOrDefault();


            if (tableName.IsNullOrEmpty())
                tableName = typeof(T).Name.ToLowerInvariant().Pluralize();

            builder.Add("DELETE FROM");
            builder.Add(tableName);

            IList<Parameter> parameters = new List<Parameter>();

            if (expression != null)
                return ExpressionTreeWalker<T>(expression, builder, parameters, ormType, timeout);

            builder.Add("WHERE");
            switch (ormType)
            {
                case OrmType.Custom:
                case OrmType.Dapper:
                    builder.Add(primaryProp.Name + "=@" + primaryProp.Name);
                    break;
                case OrmType.PetaPoco:
                    builder.Add(new List<string> { primaryProp.Name }.ToPetaPocoValues(true, parameters.Count));
                    break;
                case OrmType.SubSonic:
                    builder.Add(new List<string> { primaryProp.Name }.ToPetaPocoValues(true, parameters.Count));
                    break;
                case OrmType.EntityFramework:
                default:
                    return null;
            }
            parameters.Add(new Parameter { Name = primaryProp.Name, Value = primaryProp.GetValue(item), Type = DbTypes.TypeMap[primaryProp.PropertyType] });
            return new SqlDbCommand(string.Join(" ", builder), parameters, timeout);
        }

        #endregion

        #region Deprecate Query

        public static DQueryBase<T> Deprecate<T>()
        {
            return new DQueryDeprecate<T>(default(T));
        }

        public static SqlDbCommand GetDeprecateQuery<T>(T item, OrmType ormType = OrmType.Dapper)
        {
            return GetDeprecateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType);
        }

        public static SqlDbCommand GetDeprecateQuery<T>(T item, int timeout, OrmType ormType = OrmType.Dapper)
        {
            return GetDeprecateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, null, null, timeout);
        }

        public static SqlDbCommand GetDeprecateQuery<T>(T item, string primaryKeyColumn, OrmType ormType = OrmType.Dapper)
        {
            return GetDeprecateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, primaryKeyColumn);
        }

        public static SqlDbCommand GetDeprecateQuery<T>(T item, string primaryKeyColumn, int timeout, OrmType ormType = OrmType.Dapper)
        {
            return GetDeprecateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), item, ormType, primaryKeyColumn, null, timeout);
        }

        public static SqlDbCommand GetDeprecateQuery<T>(Expression<Func<T, bool>> expression, OrmType ormType = OrmType.Dapper)
        {
            return GetDeprecateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), default(T), ormType, null, expression);
        }

        public static SqlDbCommand GetDeprecateQuery<T>(Expression<Func<T, bool>> expression, int timeout, OrmType ormType = OrmType.Dapper)
        {
            return GetDeprecateQuery<T>(typeof(T).Name.ToLowerInvariant().Pluralize(), default(T), ormType, null, expression, timeout);
        }

        public static SqlDbCommand GetDeprecateQuery<T>(string tableName, T item, OrmType ormType = OrmType.Dapper, string primaryKeyColumn = "Id", Expression<Func<T, bool>> expression = null, int timeout = 15, string defaultPrimaryColumn = "Id")
        {
            List<string> builder = new List<string>();
            PropertyInfo[] props = PropertyCache.Resolve<T>();

            PropertyInfo primaryProp = null;

            if (primaryKeyColumn.IsNotNullOrEmpty())
                primaryProp = props.Where(x => x.Name.Equals(primaryKeyColumn)).FirstOrDefault();
            else
                primaryProp = props.Where(x => x.Name.Equals(defaultPrimaryColumn)).FirstOrDefault();


            if (tableName.IsNullOrEmpty())
                tableName = typeof(T).Name.ToLowerInvariant().Pluralize();

            builder.Add("UPDATE");
            builder.Add(tableName);
            builder.Add("SET IsDeprecated=1");

            IList<Parameter> parameters = new List<Parameter>();

            if (expression != null)
                return ExpressionTreeWalker<T>(expression, builder, parameters, ormType, timeout);

            builder.Add("WHERE");
            switch (ormType)
            {
                case OrmType.Custom:
                case OrmType.Dapper:
                    builder.Add(primaryProp.Name + "=@" + primaryProp.Name);
                    break;
                case OrmType.PetaPoco:
                    builder.Add(new List<string> { primaryProp.Name }.ToPetaPocoValues(true, parameters.Count));
                    break;
                case OrmType.SubSonic:
                    builder.Add(new List<string> { primaryProp.Name }.ToSubSonicValues(true, parameters.Count));
                    break;
                case OrmType.EntityFramework:
                default:
                    return null;
            }
            parameters.Add(new Parameter { Name = primaryProp.Name, Value = primaryProp.GetValue(item), Type = DbTypes.TypeMap[primaryProp.PropertyType] });
            return new SqlDbCommand(string.Join(" ", builder), parameters, timeout);
        }

        #endregion

        #endregion

        #region Private Methods

        private static SqlDbCommand ExpressionTreeWalker<T>(Expression<Func<T, bool>> expression, List<string> builder, IList<Parameter> parameters, OrmType ormType = OrmType.Dapper, int timeout = 15, int take = 0, int skip = 0)
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
                    switch (ormType)
                    {
                        case OrmType.Custom:
                        case OrmType.Dapper:
                            builder.Add(item.LinkingOperator + " " + item.PropertyName + " " + item.QueryOperator + " @" + item.PropertyName);
                            break;
                        case OrmType.SubSonic:
                        case OrmType.PetaPoco:
                            builder.Add(item.LinkingOperator + " " + item.PropertyName + " " + item.QueryOperator + " @" + parameterOffset.ToString());
                            break;
                        case OrmType.EntityFramework:
                        default:
                            break;
                    }
                }
                else
                {
                    switch (ormType)
                    {
                        case OrmType.Custom:
                        case OrmType.Dapper:
                            builder.Add(item.PropertyName + " " + item.QueryOperator + " @" + item.PropertyName);
                            break;
                        case OrmType.SubSonic:
                        case OrmType.PetaPoco:
                            builder.Add(item.PropertyName + " " + item.QueryOperator + " @" + parameterOffset.ToString());
                            break;
                        case OrmType.EntityFramework:
                        default:
                            break;
                    }
                }

                parameters.Add(new Parameter { Name = item.PropertyName, Value = item.PropertyValue, Type = item.DatabaseType });
            }

            if (take != 0)
            {
                builder.Add("LIMIT " + take.ToString());
                if(skip != 0)
                {
                    builder.Add("OFFSET " + skip.ToString());
                }
            }

            return new SqlDbCommand(string.Join(" ", builder).TrimEnd(), parameters, timeout);
        }

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
