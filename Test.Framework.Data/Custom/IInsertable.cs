using System;
using System.Data;

namespace Test.Framework.Data
{
    public interface IInsertable<T> : IConvertable<T>
    {
        string InsertSql();
        void ApplyInsert(T instance, IDbCommand command);
    }
}
