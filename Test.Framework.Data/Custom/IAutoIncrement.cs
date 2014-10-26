using System;

namespace Test.Framework.DataAccess
{
    public interface IAutoIncrement<T> : IConvertable<T>
    {
        void ApplyAutoNumber(T instance, int newId);
    }
}
