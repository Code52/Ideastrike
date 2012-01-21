using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Ideastrike.Nancy.Models.ViewModels;
using Nancy;
using Ideastrike.Nancy.Models.Repositories;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;
        private readonly ISettingsRepository _settings;

        public IdeaModule(IIdeaRepository ideas, ISettingsRepository settings)
            : base("/idea")
        {
            _ideas = ideas;
            _settings = settings;

            Get["/new"] = _ => View["Idea/New", new
            {
                Title = string.Format("New Idea - {0}", _settings.Title),
                Ideas = _ideas.GetAll()
            }];

            // edit an existing idea
            Get["/{id}/edit"] = parameters =>
            {
                int id = parameters.id;
                var idea = _ideas.Get(id);

                if (idea == null)
                    return View["404"];

                return View["Idea/Edit", new
                {
                    Title = string.Format("Edit Idea: '{0}' - {1}", idea.Title, _settings.Title),
                    PopularIdeas = _ideas.GetAll(),
                    Idea = idea
                }];
            };

            // view 
            Get["/{id}"] = parameters =>
                               {
                                   int id = parameters.id;
                                   var idea = _ideas.Get(id);
                                   if (idea == null)
                                       return View["404"];

                                   var viewModel = new IdeaViewModel(idea) { UserHasVoted = false };

                                   return View["Idea/Index",
                                       new
                                       {
                                           Title = string.Format("{0} - {1}", idea.Title, _settings.Title),
                                           Idea = viewModel,
                                           UserId = 2 // TODO: not hard-code these
                                       }];
                               };

            // save result of edit to database
            Post["/{id}/edit"] = parameters =>
            {
                int id = parameters.id;
                var idea = _ideas.Get(id);
                if (idea == null)
                    return View["404"];

                idea.Title = Request.Form.Title;
                idea.Description = Request.Form.Description;

                _ideas.Update(idea);

                return Response.AsRedirect(string.Format("/idea/{0}", idea.Id));
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

                ideas.Add(i);

                return Response.AsRedirect("/idea/" + i.Id);
            };

            // someone else votes for the idea
            // NOTE: should this be a POST instead of a GET?
            Get["/{id}/vote/{userid}"] = parameters =>
            {
                int votes = ideas.Vote(parameters.id, parameters.userid, 1);

                return Response.AsJson(new
                                {
                                    Status = "OK",
                                    NewVotes = votes
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
                ideas.Delete(id);
                // TODO: return a JSON result?
                return string.Format("Deleted Item {0}", id);
            };
        }
    }
}