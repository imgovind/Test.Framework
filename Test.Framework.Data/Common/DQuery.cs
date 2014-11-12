using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Data
{
    public static class DQuery<T>
        where T : class
    {
        public static DQueryInsert<T> Insert(T item, int timeout = 15)
        {
            return new DQueryInsert<T>(item);
        }

        public static DQuerySelect<T> Select(int timeout = 15)
        {
            return new DQuerySelect<T>(default(T));
        }

        public static DQueryUpdate<T> Update(T item, int timeout = 15)
        {
            return new DQueryUpdate<T>(item);
        }

        public static DQueryDelete<T> Delete(T item, int timeout = 15)
        {
            return new DQueryDelete<T>(item);
        }

        public static DQueryDeprecate<T> Deprecate(T item, int timeout = 15)
        {
            return new DQueryDeprecate<T>(item);
        }
    }

    public static class DQueryExtensions
    {
        #region Insert

        public static SqlDbCommand ToCommand<T>(this DQueryInsert<T> queryBase, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetInsertQuery<T>(queryBase.item, queryBase.timeout, ormType);
        }

        public static SqlDbCommand ToCommand<T>(this DQueryInsert<T> queryBase, bool IsAutoIncrement, string primaryKeyColumn = "Id", OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetInsertQuery<T>(queryBase.item, IsAutoIncrement, primaryKeyColumn, queryBase.timeout, ormType);
        }
        
        #endregion

        #region Select
        public static SqlDbCommand ToCommand<T>(this DQuerySelect<T> queryBase)
        {
            return DynamicQuery.GetSelectQuery<T>(queryBase.timeout);
        }

        public static SqlDbCommand Where<T>(this DQuerySelect<T> queryBase, Expression<Func<T, bool>> expression = null)
        {
            return DynamicQuery.GetSelectQuery<T>(queryBase.timeout, expression);
        }

        public static SqlDbCommand Where<T>(this DQuerySelect<T> queryBase, Expression<Func<T, bool>> expression, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetSelectQuery<T>(queryBase.timeout, expression, ormType);
        } 

        #endregion

        #region Update

        public static SqlDbCommand ToCommand<T>(this DQueryUpdate<T> queryBase, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetUpdateQuery<T>(queryBase.item, queryBase.timeout, null, ormType);
        }

        public static SqlDbCommand ToCommand<T>(this DQueryUpdate<T> queryBase, string primaryKeyColumn, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetUpdateQuery<T>(queryBase.item, primaryKeyColumn, queryBase.timeout, null, ormType);
        }

        public static SqlDbCommand Where<T>(this DQueryUpdate<T> queryBase, Expression<Func<T, bool>> expression, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetUpdateQuery<T>(queryBase.item, expression, queryBase.timeout, null, ormType);
        }

        public static SqlDbCommand Where<T>(this DQueryUpdateExclude<T> queryBase, Expression<Func<T, bool>> expression, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetUpdateQuery<T>(queryBase.update.item, expression, queryBase.excludedColumns, ormType);
        }

        public static DQueryUpdateExclude<T> Exclude<T>(this DQueryUpdate<T> queryBase, HashSet<string> excludedColumns)
        {
            return new DQueryUpdateExclude<T>(queryBase, excludedColumns);
        }

        #endregion

        #region Delete

        public static SqlDbCommand ToCommand<T>(this DQueryDelete<T> queryBase, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetDeleteQuery<T>(queryBase.item, queryBase.timeout, ormType);
        }

        public static SqlDbCommand ToCommand<T>(this DQueryDelete<T> queryBase, string primaryKeyColumn, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetDeleteQuery<T>(queryBase.item, primaryKeyColumn, queryBase.timeout, ormType);
        }

        public static SqlDbCommand Where<T>(this DQueryDelete<T> queryBase, Expression<Func<T, bool>> expression, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetDeleteQuery<T>(expression, queryBase.timeout, ormType);
        }
        
        #endregion

        #region Deprecate

        public static SqlDbCommand ToCommand<T>(this DQueryDeprecate<T> queryBase, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetDeprecateQuery<T>(queryBase.item, queryBase.timeout, ormType);
        }

        public static SqlDbCommand ToCommand<T>(this DQueryDeprecate<T> queryBase, string primaryKeyColumn, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetDeprecateQuery<T>(queryBase.item, primaryKeyColumn, queryBase.timeout, ormType);
        }

        public static SqlDbCommand Where<T>(this DQueryDeprecate<T> queryBase, Expression<Func<T, bool>> expression, OrmType ormType = OrmType.Dapper)
        {
            return DynamicQuery.GetDeprecateQuery<T>(expression, queryBase.timeout, ormType);
        }
        
        #endregion

        //public static SqlDbCommand Where<T>(this DQueryBase<T> queryBase, Expression<Func<T, bool>> expression = null)
        //{
        //    switch (queryBase.action)
        //    {
        //        case DQueryAction.Insert:
        //            return DynamicQuery.GetInsertQuery<T>(queryBase.item);
        //        case DQueryAction.Select:
        //            return DynamicQuery.GetSelectQuery<T>(expression);
        //        case DQueryAction.Update:
        //            if (expression == null)
        //                return DynamicQuery.GetUpdateQuery<T>(queryBase.item, false);
        //            else
        //                return DynamicQuery.GetUpdateQuery<T>(queryBase.item, expression);
        //        case DQueryAction.Delete:
        //            if (expression == null)
        //                return DynamicQuery.GetDeleteQuery<T>(queryBase.item, false);
        //            else
        //                return DynamicQuery.GetDeleteQuery<T>(expression);
        //        case DQueryAction.Deprecate:
        //            if (expression == null)
        //                return DynamicQuery.GetDeprecateQuery<T>(queryBase.item, false);
        //            else
        //                return DynamicQuery.GetDeprecateQuery<T>(expression);
        //        default:
        //            return null;
        //    }
        //}

        //public static SqlDbCommand Where<T>(this DQueryBase<T> queryBase, bool IsQueryForPetaPoco = false, Expression<Func<T, bool>> expression = null)
        //{
        //    switch (queryBase.action)
        //    {
        //        case DQueryAction.Insert:
        //            return DynamicQuery.GetInsertQuery<T>(queryBase.item, IsQueryForPetaPoco);
        //        case DQueryAction.Select:
        //            return DynamicQuery.GetSelectQuery<T>(expression, IsQueryForPetaPoco);
        //        case DQueryAction.Update:
        //            if (expression == null)
        //                return DynamicQuery.GetUpdateQuery<T>(queryBase.item, IsQueryForPetaPoco);
        //            else
        //                return DynamicQuery.GetUpdateQuery<T>(queryBase.item, expression, IsQueryForPetaPoco);
        //        case DQueryAction.Delete:
        //            if (expression == null)
        //                return DynamicQuery.GetDeleteQuery<T>(queryBase.item, IsQueryForPetaPoco);
        //            else
        //                return DynamicQuery.GetDeleteQuery<T>(expression, IsQueryForPetaPoco);
        //        case DQueryAction.Deprecate:
        //            if (expression == null)
        //                return DynamicQuery.GetDeprecateQuery<T>(queryBase.item, IsQueryForPetaPoco);
        //            else
        //                return DynamicQuery.GetDeprecateQuery<T>(expression, IsQueryForPetaPoco);
        //        default:
        //            return null;
        //    }
        //}
    }

    public class DQueryBase<T>
    {
        public T item { get; set; }
        public DQueryAction action { get; set; }
        public int timeout { get; set; }

        public DQueryBase(T item, DQueryAction action, int timeout)
        {
            // TODO: Complete member initialization
            this.item = item;
            this.action = action;
            this.timeout = timeout;
        }
    }

    public class DQueryInsert<T> : DQueryBase<T>
    {
        public DQueryInsert(T item, int timeout = 15)
            : base(item, DQueryAction.Insert, timeout)
        {
        }
    }

    public class DQuerySelect<T> : DQueryBase<T>
    {
        public DQuerySelect(T item, int timeout = 15)
            : base(item, DQueryAction.Select, timeout)
        {
        }
    }

    public class DQueryUpdate<T> : DQueryBase<T>
    {
        public DQueryUpdate(T item, int timeout = 15)
            : base(item, DQueryAction.Update, timeout)
        {
        }
    }

    public class DQueryUpdateExclude<T>
    {
        public DQueryUpdate<T> update { get; set; }
        public HashSet<string> excludedColumns { get; set; }

        public DQueryUpdateExclude(DQueryUpdate<T> update, HashSet<string> excludedColumns)
        {
            this.update = update;
            this.excludedColumns = excludedColumns;
        }
    }

    public class DQueryDelete<T> : DQueryBase<T>
    {
        public DQueryDelete(T item, int timeout = 15)
            : base(item, DQueryAction.Delete, timeout)
        {
        }
    }

    public class DQueryDeprecate<T> : DQueryBase<T>
    {
        public DQueryDeprecate(T item, int timeout = 15)
            : base(item, DQueryAction.Deprecate, timeout)
        {
        }
    }

    public enum DQueryAction
    { 
        Insert = 0,
        Select = 1,
        Update = 2,
        Delete = 3,
        Deprecate = 4
    }
}
