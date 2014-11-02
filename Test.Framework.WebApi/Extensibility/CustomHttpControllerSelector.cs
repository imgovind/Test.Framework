using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using Test.Framework.Extensions;

namespace Test.Framework.WebApi
{
    public class CustomHttpControllerSelector : DefaultHttpControllerSelector, IHttpControllerSelector
    {
        #region Contructor and Private Members

        private readonly HttpConfiguration _configuration;
        private readonly HashSet<string> _duplicates;
        private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> _controllers;

        public CustomHttpControllerSelector(HttpConfiguration config)
            : base(config)
        {
            _configuration = config;
            _duplicates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _controllers = new Lazy<Dictionary<string, HttpControllerDescriptor>>(InitializeControllerDictionary);
        }

        #endregion

        #region Public Methods

        public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return base.GetControllerMapping();
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllers = GetControllerMapping();

            //If Controller Mapping is failing - the app has a serious error
            if (controllers == null)
                return base.SelectController(request);

            var routeData = request.GetRouteData();

            //If RouteData is empty, the routes must have some critical error
            if (routeData == null)
                return base.SelectController(request);

            string controllerName = string.Empty;
            if (routeData.Values.ContainsKey("controller"))
            {
                controllerName = (string)routeData.Values["controller"];
            }
            else
            {
                var subRoutes = (IEnumerable<IHttpRouteData>)request.GetRouteData().Values["MS_SubRoutes"]; ;
                var route = subRoutes.First().Route;
            }

            //If the controller name is not present, please check the routing configuration
            if (controllerName.IsNullOrEmpty())
                return base.SelectController(request);

            HttpControllerDescriptor descriptor;

            //If the given controller cannot be found in the executing assembly then something critical has happened
            if (!controllers.TryGetValue(controllerName, out descriptor))
                return base.SelectController(request);

            //var version = GetVersionFromQueryString(request);
            //var version = GetVersionFromHeader(request);
            //var version = GetVersionFromAcceptHeaderVersion(request);
            var version = GetVersionFromMediaType(request);

            //If the version is empty - the request is erroneous
            if (version.IsNullOrEmpty())
                return base.SelectController(request);

            var versionedControllerName = string.Concat(controllerName, "V", version);

            HttpControllerDescriptor versionedDescriptor;

            //If the versioned descriptor is empty, then make sure if there controller is properly named with a capital V
            if (!controllers.TryGetValue(versionedControllerName, out versionedDescriptor))
                return descriptor;

            return versionedDescriptor;
        }

        #endregion

        #region Private Methods
        private string GetVersionFromMediaType(HttpRequestMessage request)
        {
            var accept = request.Headers.Accept;
            var ex = new Regex(@"application\/vnd\.testangular\.v([0-9]+)\+json", RegexOptions.IgnoreCase);

            foreach (var mime in accept)
            {
                var match = ex.Match(mime.MediaType);
                if (match != null)
                {
                    return match.Groups[1].Value;
                }
            }

            return "1";
        }

        private string GetVersionFromAcceptHeaderVersion(HttpRequestMessage request)
        {
            var accept = request.Headers.Accept;

            foreach (var mime in accept)
            {
                if (mime.MediaType == "application/json")
                {
                    var value = mime.Parameters
                                    .Where(v => v.Name.Equals("version", StringComparison.OrdinalIgnoreCase))
                                    .FirstOrDefault();

                    return value.Value;
                }
            }

            return "1";
        }

        private string GetVersionFromHeader(HttpRequestMessage request)
        {
            const string HEADER_NAME = "X-CountingKs-Version";

            if (request.Headers.Contains(HEADER_NAME))
            {
                var header = request.Headers.GetValues(HEADER_NAME).FirstOrDefault();
                if (header != null)
                {
                    return header;
                }
            }

            return "1";
        }

        private string GetVersionFromQueryString(HttpRequestMessage request)
        {
            var query = ParseQueryString(request.RequestUri.Query);

            var version = query["v"];
            if (version != null)
            {
                return version;
            }

            return "1";
        }
        private static NameValueCollection ParseQueryString(string s)
        {
            NameValueCollection nvc = new NameValueCollection();
            // remove anything other than query string from url
            if (s.Contains("?"))
            {
                s = s.Substring(s.IndexOf('?') + 1);
            }
            foreach (string vp in Regex.Split(s, "&"))
            {
                string[] singlePair = Regex.Split(vp, "=");
                if (singlePair.Length == 2)
                {
                    nvc.Add(singlePair[0], singlePair[1]);
                }
                else
                {
                    // only one key with no value specified in query string
                    nvc.Add(singlePair[0], string.Empty);
                }
            }
            return nvc;
        }
        private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
        {
            var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

            // Create a lookup table where key is "namespace.controller". The value of "namespace" is the last
            // segment of the full namespace. For example:
            // MyApplication.Controllers.V1.ProductsController => "V1.Products"
            IAssembliesResolver assembliesResolver = _configuration.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver controllersResolver = _configuration.Services.GetHttpControllerTypeResolver();

            ICollection<Type> controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);

            foreach (Type t in controllerTypes)
            {
                var segments = t.Namespace.Split(Type.Delimiter);

                // For the dictionary key, strip "Controller" from the end of the type name.
                // This matches the behavior of DefaultHttpControllerSelector.
                var controllerName = t.Name.Remove(t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length);

                var key = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", segments[segments.Length - 1], controllerName);

                // Check for duplicate keys.
                if (dictionary.Keys.Contains(key))
                {
                    _duplicates.Add(key);
                }
                else
                {
                    dictionary[key] = new HttpControllerDescriptor(_configuration, t.Name, t);
                }
            }

            // Remove any duplicates from the dictionary, because these create ambiguous matches. 
            // For example, "Foo.V1.ProductsController" and "Bar.V1.ProductsController" both map to "v1.products".
            foreach (string s in _duplicates)
            {
                dictionary.Remove(s);
            }
            return dictionary;
        }

        #endregion
    }

}
