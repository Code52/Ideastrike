using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(IIdeaRepository ideas)
        {
            Get["/"] = _ => View["Home/Index", ideas.GetAll()];

        }
    }
}