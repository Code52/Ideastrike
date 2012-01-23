using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Ideastrike.Nancy.Modules
{
    public class ApiSecuredModule : NancyModule
    {
        public ApiSecuredModule(IIdeaRepository ideas, ISettingsRepository settings) : base("/api") {
            this.Before.AddItemToEndOfPipeline(ctx => {
                if (ctx.CurrentUser == null)
                    return HttpStatusCode.Unauthorized;
                return null; 
            });

            Post["/ideas"] = _ => {
                var model = this.Bind<EditIdeaModel>();

                var idea = new Idea {
                    Title = model.title,
                    Description = model.description,
                    Time = DateTime.UtcNow,
                    Author = (User)Context.CurrentUser,
                    Status = settings.IdeaStatusDefault
                };
                ideas.Add(idea);

                return HttpStatusCode.Created;  // TODO: Should return either the generated id or the json body
            };

            Put["/ideas/{id}"] = _ => {
                var model = this.Bind<EditIdeaModel>();
                int id = _.id;
                var idea = ideas.Get(id);
                if (idea == null)
                    return HttpStatusCode.NotFound;
                if (model.title != null)
                    idea.Title = model.title;
                if (model.description != null)
                    idea.Description = model.description;
                ideas.Edit(idea);

                return HttpStatusCode.OK;
            };
        }
    }

    public class EditIdeaModel
    {
        public string title { get; set; }
        public string description { get; set; }
    }
}