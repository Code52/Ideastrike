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
        private readonly ISettingsRepository _settings;

        public HomeModule(IIdeaRepository ideas, ISettingsRepository settings)
        {
            _ideas = ideas;
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
            dynamic pageData = new
            {
                Ideas = ideas,
                Selected = selected,
                Title = _settings.Title,
                WelcomeMessage = _settings.WelcomeMessage,
                ErrorMessage = ErrorMessage,
                IsLoggedIn = Context.IsLoggedIn(),
                UserName = Context.Username(),
            };

            return View["Home/Index", pageData];
        }
    }
}