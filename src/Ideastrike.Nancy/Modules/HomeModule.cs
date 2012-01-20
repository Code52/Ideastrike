using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(IIdeaRepository ideas, ISettingsRepository settings)
        {
            Get["/"] = _ => View["Home/Index", new
                                                   {
                                                       Ideas= ideas.GetAll(), 
                                                       Title = settings.Title,
                                                       WelcomeMessage = MarkdownHelper.Markdown(settings.WelcomeMessage)
                                                   }];

        }
    }
}