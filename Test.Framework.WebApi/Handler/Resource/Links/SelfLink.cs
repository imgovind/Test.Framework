
namespace Test.Framework.Handlers.Resource
{
    /// <summary>
    /// Conveys an identifier for the link's context.
    /// </summary>
    public class SelfLink : Link
    {
        public const string Relation = "self";

        public SelfLink(string href, string method = "GET", string title = null) 
            : base(Relation, href, method, title)
        {
        }
    }
}
