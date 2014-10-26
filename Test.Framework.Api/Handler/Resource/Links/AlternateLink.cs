
namespace Test.Framework.Handlers.Resource
{
    /// <summary>
    /// Refers to a substitute for this context
    /// </summary>
    public class AlternateLink : Link
    {
        public const string Relation = "alternate";

        public AlternateLink(string href, string method = "GET", string title = null)
            : base(Relation, href, method, title)
        {
        }
    }
}
