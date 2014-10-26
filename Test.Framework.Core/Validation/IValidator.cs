using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public interface IValidator
    {
        bool IsValid { get; }
        string Message { get; }
        void Validate();
    }
}
