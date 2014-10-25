
namespace Test.Framework.Handlers.Resource
{
    /// <summary>
    /// Refers to a resource that can be used to edit the link's context.
    /// </summary>
    public class EditLink : Link
    {
        public const string Relation = "edit";

        public EditLink(string href, string method = "PUT", string title = null)
            : base(Relation, href, method, title)
        {
        }
    }
}
