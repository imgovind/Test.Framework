using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="System.Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the <paramref name="type"/> implements <typeparamref name="T"/>.
        /// </summary>
        public static bool Implements<T>(this Type type)
        {
            Ensure.Argument.IsNotNull(type, "type");
            return typeof(T).IsAssignableFrom(type);
        }
    }
}
