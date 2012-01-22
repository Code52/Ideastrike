using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Ideastrike.Nancy.Models.ViewModels;
using Nancy;

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

                var viewModel = new IdeaViewModel(idea) { UserHasVoted = false };

                dynamic model = new
                {
                    Title = string.Format("{0} - {1}", idea.Title, _settings.Title),
                    Idea = viewModel,
                    IsLoggedIn = Context.IsLoggedIn(),
                    UserName = Context.Username(),
                };

                return View["Idea/Index", model];
            };
        }
    }
}