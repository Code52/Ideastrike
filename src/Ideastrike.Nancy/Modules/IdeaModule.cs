using System;
using Ideastrike.Nancy.Models;
using System.Linq;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaModule : NancyModule
    {
        public IdeaModule(IIdeaRepository ideas)
            : base("/idea")
        {
            Get["/new"] = _ => View["Idea/New", ideas.GetAllIdeas()];

            // edit an existing idea
            Get["/{id}/edit"] = parameters =>
            {
                // TODO: get result result in database 
                int id = parameters.id;
                Idea idea = ideas.GetIdea(id);

                return View["Idea/Edit", idea];
            };

            // view 
            Get["/{id}"] = parameters =>
                               {
                                   int id = parameters.id;
                                   Idea idea = ideas.GetIdea(id);
								   if (idea == null)
                                       return View["Shared/404"];

                                   return View["Idea/Index", new { Idea = idea, 
                                       UserId = 2, // TODO: not hard-code these
                                       UserHasVoted = false }];
                               };

            // save result of edit to database
            Post["/{id}/edit"] = _ =>
            {
                // TODO: update result in database 
                // TODO: return redirect
                return View["/idea/index", 1];
            };

            // save result of create to database
            Post["/new"] = _ =>
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

            // someone else votes for the idea
            // NOTE: should this be a POST instead of a GET?
            Get["/{id}/vote/{userid}"] = parameters =>
            {
                Idea idea = ideas.GetIdea(parameters.id);
                ideas.Vote(idea, parameters.userid, 1);

                return Response.AsJson(new
                                {
                                    Status = "OK",
                                    NewVotes = idea.Votes.Sum(v => v.Value)
                                });
            };

            // the user decides to repeal his vote
            // NOTE: should this be a POST instead of a GET?
            Get["/{id}/unvote/{userid}"] = parameters =>
            {
                // TODO: implementation 

                return Response.AsJson(new
                {
                    Status = "Error",
                    
                });
            };

            // should this be a POST instead of a GET?
            
            Get["/{id}/delete"] = parameters =>
            {
                int id = parameters.id;
                ideas.DeleteIdea(id);
                // TODO: return a JSON result?
                return string.Format("Deleted Item {0}", id);
            };
        }
    }
}