using System.Collections.Generic;
using System.Linq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class HomeModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;
        private readonly IUserRepository _users;
        private readonly ISettingsRepository _settings;

        public HomeModule(IIdeaRepository ideas, IUserRepository users, ISettingsRepository settings)
        {
            _ideas = ideas;
            _users = users;
            _settings = settings;
            Get["/"] = _ => ListIdeas(_ideas.GetAll(), SelectedTab.Popular, "");
            Get["/top"] = _ => ListIdeas(_ideas.GetAll().OrderByDescending(i => i.Votes.Count), SelectedTab.Hot, "");
            Get["/new"] = _ => ListIdeas(_ideas.GetAll().OrderByDescending(i => i.Time), SelectedTab.New, "");
            Get["/login"] = _ =>
                                {
                                    return ListIdeas(_ideas.GetAll(), SelectedTab.Popular,
                                                     "You need to login to do that");
                                };
        }

        public Response ListIdeas(IEnumerable<Idea> ideas, SelectedTab selected, string ErrorMessage)
        {
            foreach(var i in ideas)
            {
                User user = Context.GetCurrentUser(_users);
                if (user != null)
                {
                    if (i.Votes.Any(u => u.UserId == user.Id))
                        i.UserHasVoted = true;

                }
            }

            var m = Context.Model(_settings.Title);
            m.Name = _settings.Name;
            m.WelcomeMessage = _settings.WelcomeMessage;
            m.Ideas = ideas;
            m.Selected = selected;
            m.ErrorMessage = ErrorMessage;

            return View["Home/Index", m];
        }
    }
}