using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Test.Framework.Session.InProc
{
    public class InProcSessionStore : ISessionStore
    {
        #region Members

        internal HttpContext Current
        {
            get
            {
                Ensure.Argument.IsNotNull(HttpContext.Current, "HttpContext.Current");
                Ensure.Argument.IsNotNull(HttpContext.Current.Session, "HttpContext.Current.Session");

                if (HttpContext.Current.Session.Mode == SessionStateMode.Off)
                {
                    throw new Exception("Session elements cannot be added when session is disabled.");
                }
                return HttpContext.Current;
            }
        }

        #endregion

        #region Constructors

        public InProcSessionStore()
        {
        }

        #endregion

        #region ISessionStore Members

        public string Id
        {
            get
            {
                return Current.Session.SessionID.ToString();
            }
        }

        public T Get<T>(string key)
        {
            if (Current.Session[key] == null)
            {
                return default(T);
            }
            else
            {
                return (T)Current.Session[key];
            }
        }

        public void Add<T>(string key, T value)
        {
            Current.Session.Add(key, value);
        }

        public void Remove(string key)
        {
            Current.Session.Remove(key);
        }

        public bool Contains(string key)
        {
            return Current.Session[key] != null;
        }

        public void Abandon()
        {
            Current.Session.Abandon();
        }

        public void Clear()
        {
            Current.Session.Clear();
        }

        #endregion
    }
}
