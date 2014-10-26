using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Session
{
    public interface ISessionStore
    {
        string Id { get; }
        T Get<T>(string key);
        bool Contains(string key);
        void Add<T>(string key, T value);
        void Remove(string key);
        void Abandon();
        void Clear();
    }
}
