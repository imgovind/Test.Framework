using System;

namespace Test.Framework.Data
{
    public interface IAutoIncrement<T> : IConvertable<T>
    {
        void ApplyAutoNumber(T instance, int newId);
    }
}
