using System;
using System.Collections.Generic;
using System.Data;

namespace Test.Framework.Data
{
    public interface ISelectable<T> : IConvertable<T>
    {
        T ApplySelect(DataReader reader);
        T ApplySelect(DataReader reader, ISet<string> columns);
    }
}
