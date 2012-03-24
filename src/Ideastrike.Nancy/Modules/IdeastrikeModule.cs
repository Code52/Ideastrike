using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class IdeastrikeModule : NancyModule
    {
        public IdeastrikeModule(string path)
            : base(path)
        {

        }

        public bool Administrator(NancyContext context)
        {
            return false;
        }

        public bool Authenticated(NancyContext context)
        {
            return context.CurrentUser != null;
        }

        public bool Moderator(NancyContext context)
        {
            return false;
        }
    }
}