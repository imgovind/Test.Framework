using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace Test.Framework.Api
{
    /// <summary>
    /// Explores the controllers and finds the hierarchy and relationship and fills the dictionary
    /// </summary>
    public interface IHierarchyExplorer
    {
        void Explore(HttpConfiguration configuration, IDictionary<string, IEnumerable<string>> controllerNameHierarchy);
    }
}
