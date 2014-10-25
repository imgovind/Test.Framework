
namespace Test.Framework.Handlers.Resource
{
    /// <summary>
    /// An IRI that refers to the furthest following resource in a series of resources.
    /// </summary>
    public class LastLink : Link
    {
        public const string Relation = "last";

        public LastLink(string href, string method = "GET", string title = null)
            : base(Relation, href, method, title)
        {
        }
    }
}
