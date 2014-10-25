using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Test.Framework.Session.Memcache
{
    public sealed class MemcacheSessionStateStoreProvider : SessionStateStoreProviderBase
    {
        #region Private Members

        private static readonly IMemcachedClient cache = SessionBucketActivator.Cache;

        #endregion

        #region SessionStateStoreProviderBase Members

        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            return SessionActor.CreateNewStoreData(context, timeout);
        }

        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            cache.Store(StoreMode.Set, id, new MemcacheHolder(null, false, DateTime.Now, 0, 1), new TimeSpan(0, timeout, 0));
        }

        public override void Dispose()
        {
        }

        public override void EndRequest(HttpContext context)
        {
        }

        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
        {
            DateTime setTime = DateTime.Now;
            SessionStateStoreData item = null;

            lockId = null;
            locked = false;
            actionFlags = 0;
            lockAge = TimeSpan.Zero;

            MemcacheHolder holder = cache.Get<MemcacheHolder>(id);
            if (holder != null)
            {
                if (!holder.Locked)
                {
                    holder.LockId++;
                    holder.SetTime = setTime;
                    cache.Store(StoreMode.Set, id, holder, new TimeSpan(0, 0, Convert.ToInt32(FrameworkSettings.Session.SessionTimeoutInMinutes), 0, 0));

                    lockId = holder.LockId;
                    lockAge = holder.LockAge;
                    actionFlags = (SessionStateActions)holder.ActionFlag;

                    if (actionFlags == SessionStateActions.InitializeItem)
                    {
                        item = SessionActor.CreateNewStoreData(context, Convert.ToInt32(FrameworkSettings.Session.SessionTimeoutInMinutes));
                    }
                    else
                    {
                        item = SessionActor.Deserialize(context, holder.Content, Convert.ToInt32(FrameworkSettings.Session.SessionTimeoutInMinutes));
                    }
                }
                else
                {
                    locked = true;
                    lockId = holder.LockId;
                    lockAge = holder.LockAge;
                    actionFlags = (SessionStateActions)holder.ActionFlag;
                }
            }
            return item;
        }

        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
        {
            DateTime setTime = DateTime.Now;
            SessionStateStoreData item = null;

            lockId = null;
            locked = false;
            actionFlags = 0;
            lockAge = TimeSpan.Zero;

            MemcacheHolder holder = cache.Get<MemcacheHolder>(id);
            if (holder != null)
            {
                if (!holder.Locked)
                {
                    holder.LockId++;
                    holder.SetTime = setTime;
                    holder.Locked = true;
                    cache.Store(StoreMode.Set, id, holder, new TimeSpan(0, 0, Convert.ToInt32(FrameworkSettings.Session.SessionTimeoutInMinutes), 0, 0));

                    locked = true;
                    lockId = holder.LockId;
                    lockAge = holder.LockAge;
                    actionFlags = (SessionStateActions)holder.ActionFlag;
                }
                else
                {
                    locked = true;
                    lockId = holder.LockId;
                    lockAge = holder.LockAge;
                    actionFlags = (SessionStateActions)holder.ActionFlag;
                }

                if (actionFlags == SessionStateActions.InitializeItem)
                {
                    item = SessionActor.CreateNewStoreData(context, Convert.ToInt32(FrameworkSettings.Session.SessionTimeoutInMinutes));
                }
                else
                {
                    item = SessionActor.Deserialize(context, holder.Content, Convert.ToInt32(FrameworkSettings.Session.SessionTimeoutInMinutes));
                }

            }
            return item;
        }

        public override void InitializeRequest(HttpContext context)
        {
        }

        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            MemcacheHolder holder = cache.Get<MemcacheHolder>(id);

            if (holder != null)
            {
                holder.Locked = false;
                holder.LockId = (int)lockId;
                cache.Store(StoreMode.Set, id, holder);
            }
        }

        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            cache.Remove(id);
        }

        public override void ResetItemTimeout(HttpContext context, string id)
        {
            object item = cache.Get(id);

            if (item != null)
            {
                cache.Store(StoreMode.Set, id, item, new TimeSpan(0, Convert.ToInt32(FrameworkSettings.Session.SessionTimeoutInMinutes), 0));
            }
        }

        public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {
            byte[] content = null;
            DateTime setTime = DateTime.Now;

            content = SessionActor.Serialize((SessionStateItemCollection)item.Items);

            if (newItem == true)
            {
                MemcacheHolder holder = new MemcacheHolder(content, false, setTime, 0, 0);
                cache.Store(StoreMode.Add, id, holder, new TimeSpan(0, item.Timeout, 0));
            }
            else
            {
                MemcacheHolder holder = new MemcacheHolder(content, false, setTime, 0, 0);
                cache.Store(StoreMode.Set, id, holder, new TimeSpan(0, item.Timeout, 0));
            }
        }

        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            return false;
        }

        #endregion
    }
}
