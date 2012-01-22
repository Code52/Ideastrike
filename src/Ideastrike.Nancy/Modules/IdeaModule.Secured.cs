using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Nancy.Security;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaSecuredModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;
        private readonly IUserRepository _users;
        private readonly ISettingsRepository _settings;

        public IdeaSecuredModule(IIdeaRepository ideas, IUserRepository users, ISettingsRepository settings)
            : base("/idea")
        {
            _ideas = ideas;
            _settings = settings;
            _users = users;

            this.RequiresAuthentication();

            Get["/new"] = _ =>
            {
                var m = Context.Model(string.Format("New Idea - {0}", _settings.Title));
                m.Ideas = _ideas.GetAll();
                return View["Idea/New", m];
            };

            Get["/{id}/edit"] = parameters =>
            {
                int id = parameters.id;
                var idea = _ideas.Get(id);

                if (idea == null)
                    return View["404"];

                var m = Context.Model(string.Format("Edit Idea: '{0}' - {1}", idea.Title, _settings.Title));
                m.PopularIdeas = _ideas.GetAll();
                m.Idea = idea;

                return View["Idea/Edit", m];
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
                var user = _users.FindBy(u => u.UserName == Context.CurrentUser.UserName).FirstOrDefault();

                if (user == null)
                    return Response.AsRedirect("/login");

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
            Post["/{id}/vote"] = parameters =>
            {
                var user = Context.GetCurrentUser(_users);

                if (user == null)
                    return Response.AsRedirect("/login");

                int votes = ideas.Vote(parameters.id, user.Id, 1);

                return Response.AsJson(new
                                        {
                                            Status = "OK",
                                            NewVotes = votes
                                        });
            };

            // the user decides to repeal his vote
            Post["/{id}/unvote"] = parameters =>
            {
                var user = Context.GetCurrentUser(_users);
                int votes = ideas.Unvote(parameters.id, user.Id);

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