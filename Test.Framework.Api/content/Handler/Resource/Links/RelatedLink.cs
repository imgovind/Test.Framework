
namespace Test.Framework.Handlers.Resource
{
    /// <summary>
    /// Identifies a related resource.
    /// </summary>
    public class RelatedLink : Link
    {
        public const string Relation = "related";

        public RelatedLink(string href, string method = "GET", string title = null)
            : base(Relation, href, method, title)
        {
        }
    }
}
