using System.Linq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Ideastrike.Nancy.Models.ViewModels;
using Nancy;
using Nancy.ViewEngines;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Nancy.Helpers;
using Nancy.Extensions;

using Nancy.ViewEngines.Razor;

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

            Get["/{id}"] = parameters =>
            {
                int id = parameters.id;
                var idea = _ideas.Get(id);
                if (idea == null)
                    return View["404"];

                User user = Context.GetCurrentUser(_users);
                if (user != null)
                {
                    if (idea.Votes.Any(u => u.UserId == user.Id))
                        idea.UserHasVoted = true;

                }

                var viewModel = new IdeaViewModel(idea);
                var model = Context.Model(string.Format("{0} - {1}", idea.Title, _settings.Title));
                model.Idea = viewModel;
                return View["Idea/Index", model];
            };
        }
    }
}