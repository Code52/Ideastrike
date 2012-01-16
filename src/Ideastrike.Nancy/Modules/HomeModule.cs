using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => "Hello, world";
        }
    }
}