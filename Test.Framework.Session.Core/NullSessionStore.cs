using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Session
{
    public class NullSessionStore : ISessionStore
    {
        public string Id
        {
            get { return null; }
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public bool Contains(string key)
        {
            return false;
        }

        public void Add<T>(string key, T value)
        {
        }

        public void Remove(string key)
        {
        }

        public void Abandon()
        {
        }

        public void Clear()
        {
        }
    }
}
