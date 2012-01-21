using System;
using System.Collections.Generic;
using System.Linq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Ideastrike.Nancy.Models.ViewModels;
using Nancy;

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

            Get["/{id}/activity"] = parameters =>
                                        {
                                            int id = parameters.id;
                                            var idea = _ideas.Get(id);
                                            if (idea == null)
                                                return Response.AsJson(new { Status = "error" });

                                            var results = idea.Activities.Select(MapToViewModel);

                                            return Response.AsJson(new
                                            {
                                                Status = "success",
                                                Items = results
                                            });
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

                _ideas.Save();

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

            Post["/{id}/delete"] = parameters =>
            {
                int id = parameters.id;
                ideas.Delete(id);
                ideas.Save();

                // TODO: test
                return Response.AsJson(new
                {
                    Status = "Error"
                });
            };

            // someone else votes for the idea
            Post["/{id}/vote/{userid}"] = parameters =>
            {
                int votes = ideas.Vote(parameters.id, parameters.userid, 1);

                return Response.AsJson(new
                                {
                                    Status = "OK",
                                    NewVotes = votes
                                });
            };

            // the user decides to repeal his vote
            Post["/{id}/unvote/{userid}"] = parameters =>
            {
                int votes = ideas.Unvote(parameters.id, parameters.userid);

                return Response.AsJson(new
                {
                    Status = "OK",
                    NewVotes = votes
                });
            };
        }

        private static object MapToViewModel(Activity activity)
        {
            var github = activity as GitHubActivity;

            if (github != null)
                return new { template = "github", item = new GitHubActivityViewModel(github) };

            var comment = activity as Comment;
            if (comment != null)
                return new { template = "comment", item = new CommentViewModel(comment) };

            return null;
        }
    }
}