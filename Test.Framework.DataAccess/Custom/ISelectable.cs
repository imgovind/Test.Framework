using System;
using System.Collections.Generic;
using System.Data;

namespace Test.Framework.DataAccess
{
    public interface ISelectable<T> : IConvertable<T>
    {
        T ApplySelect(DataReader reader);
        T ApplySelect(DataReader reader, ISet<string> columns);
    }
}
