
namespace Test.Framework.Handlers.Resource
{
    /// <summary>
    /// Refers to a resource that can be used to edit media associated with the link's context.
    /// </summary>
    public class EditMediaLink : Link
    {
        public const string Relation = "edit-media";

        public EditMediaLink(string href, string method = "PUT", string title = null)
            : base(Relation, href, method, title)
        {
        }
    }
}
