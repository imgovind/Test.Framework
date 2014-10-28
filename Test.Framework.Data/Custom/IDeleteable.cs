using System;
using System.Data;

namespace Test.Framework.Data
{
    public interface IDeleteable<T> : IConvertable<T>
    {
        string DeleteSql();
        void ApplyDelete(T instance, IDbCommand command);
    }
}
