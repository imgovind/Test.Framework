
namespace Test.Framework.Handlers.Resource
{
    /// <summary>
    /// An IRI that refers to the furthest preceding resource in a series of resources.
    /// </summary>
    public class FirstLink : Link
    {
        public const string Relation = "first";

        public FirstLink(string href, string method = "GET", string title = null)
            : base(Relation, href, method, title)
        {
        }
    }
}
