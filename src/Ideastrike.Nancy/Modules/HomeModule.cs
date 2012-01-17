using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class HomeModule : NancyModule
    {
        IIdeaRepository IdeaRep = new IdeaRepository();
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                IdeaRep.AddIdea(new Idea { Title = "I heard you like ideas, so I put an idea in your ideas", Time = DateTime.UtcNow });
                return View["Home/Index", string.Format("Hello, world. There are {0} ideas", IdeaRep.CountIdeas())];
            };
        }
    }
}