using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Session.Memcache
{
    [Serializable]
    internal class MemcacheHolder
    {
        public MemcacheHolder(byte[] content, bool isLocked, DateTime setTime, int lockId, int actionFlag)
        {
            this.LockId = lockId;
            this.Locked = isLocked;
            this.SetTime = setTime;
            this.Content = content;
            this.ActionFlag = actionFlag;
        }

        public int LockId { get; set; }

        public bool Locked { get; set; }

        public int ActionFlag { get; set; }

        public byte[] Content { get; set; }

        public DateTime SetTime { get; set; }

        public TimeSpan LockAge { get { return DateTime.Now.Subtract(this.SetTime); } }

    }
}
