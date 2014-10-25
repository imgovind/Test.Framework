using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Handlers.Resource
{
    /// <summary>
    /// Refers to a resource that can be used to edit the link's context.
    /// </summary>
    public class RUDLink : Link
    {
        public const string Relation = "self, edit, update, delete";

        public RUDLink(string href, string method = "GET, PUT, PATCH, DELETE", string title = null)
            : base(Relation, href, method, title)
        {
        }
    }
}
