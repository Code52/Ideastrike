using System;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaModule : NancyModule
    {
        public IdeaModule(IIdeaRepository ideas) : base("/idea")
        {
            Get["/{id}"] = parameters =>
                               {
                                   int id = parameters.id;
                                   Idea idea = ideas.GetIdea(id);
                                   return View["Idea/Index", new { idea.Id, idea.Title, idea.Description, Features = idea.Features, Activities = idea.Activities }];
                               };

            Post["/"] = _ =>
            {
                var i = new Idea
                            {
                                Time = DateTime.UtcNow,
                                Title = Request.Form.Title,
                                Description = Request.Form.Description,
                            };

                ideas.AddIdea(i);

                return Response.AsRedirect("/idea/" + i.Id);
            };

            Get["/{id}/delete"] = parameters =>
            {
                int id = parameters.id;
                ideas.DeleteIdea(id);
                return string.Format("Deleted Item {0}", id);
            };
        }
    }
}