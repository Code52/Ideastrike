using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Ideastrike.Nancy.Models.ViewModels;
using Nancy;
using Nancy.Security;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;
        private readonly IUserRepository _users;
        private readonly ISettingsRepository _settings;

        public IdeaModule(IIdeaRepository ideas, IUserRepository users, ISettingsRepository settings)
            : base("/idea")
        {
            

            _ideas = ideas;
            _settings = settings;
            _users = users;

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

                                   var user = Context.CurrentUser == null ? null : _users.FindBy(u => u.UserName == Context.CurrentUser.UserName).FirstOrDefault();


                                   var viewModel = new IdeaViewModel(idea) { UserHasVoted = false };

                                   dynamic model = new
                                                       {
                                                           Title =
                                                               string.Format("{0} - {1}", idea.Title, _settings.Title),
                                                           Idea = viewModel,
                                                           UserId = Guid.Empty,
                                                       };

                                   if (user != null)
                                       model.UserId = user.Id; // TODO: not hard-code these

                                   return View["Idea/Index", model];
                               };

            // save result of edit to database
            Post["/{id}/edit"] = parameters =>
            {
                this.RequiresAuthentication();

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
                                   var user = _users.FindBy(u => u.UserName == Context.CurrentUser.UserName).FirstOrDefault();

                                   if(user == null) return Response.AsRedirect("/idea/"); //TODO: Problem looking up the user? Return an error

                                   var i = new Idea
                                               {
                                                   Author = user,
                                                   Time = DateTime.UtcNow,
                                                   Title = Request.Form.Title,
                                                   Description = Request.Form.Description,
                                               };

                                   ideas.Add(i);

                                   return Response.AsRedirect("/idea/" + i.Id);
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
        }
    }
}