using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class CacheStrings : ConfigurationElementCollection
    {
        public CacheStrings()
        {
        }

        public CacheString this[int index]
        {
            get
            {
                return (CacheString)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(CacheString cacheString)
        {
            BaseAdd(cacheString);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CacheString();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CacheString)element).Name;
        }

        public void Remove(CacheString cacheString)
        {
            BaseRemove(cacheString.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }
}
