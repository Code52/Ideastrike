using System;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaModule : NancyModule
    {
       
        public IdeaModule(IIdeaRepository ideas)
        {
            Get["/idea/{id}"] = parameters =>
            {
                var idea = ideas.GetIdea(parameters.id);
                return View["Idea/Index", new { idea.Title, idea.Description }]; 
            };

            Post["/idea"] = _ =>
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

            Get["/idea/{id}/delete"] = parameters =>
            {
                double id = parameters.id;
                ideas.DeleteIdea(id);
                return string.Format("Deleted Item {0}", id);
            };
        }
    }
}