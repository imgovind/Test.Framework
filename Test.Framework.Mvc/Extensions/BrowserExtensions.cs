using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Test.Framework.Extensions;

namespace Test.Framework.Mvc
{
    public static class BrowserExtensions
    {
        public static bool IsBrowserAllowed(this HttpBrowserCapabilities browser)
        {
            if (browser.Browser.IsEqual("IE") && browser.MajorVersion < 8)
            {
                return false;
            }

            if (browser.Browser.IsEqual("Firefox") && browser.MajorVersion < 3)
            {
                return false;
            }

            return true;
        }

        public static bool IsBrowserAllowed(this HttpBrowserCapabilitiesBase browser)
        {
            if (browser.Browser.IsEqual("IE") && browser.MajorVersion < 8)
            {
                return false;
            }

            if (browser.Browser.IsEqual("Firefox") && browser.MajorVersion < 3)
            {
                return false;
            }

            return true;
        }
    }
}
