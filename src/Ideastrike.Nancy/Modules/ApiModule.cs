using System;
using System.Data.Objects.SqlClient;
using System.Linq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Nancy.ModelBinding;

namespace Ideastrike.Nancy.Modules
{
    // The code in the API is very explicit in terms of the property names and shape of the output json
    // It does not go via the repositories, and does not simply dump models out as json
    // The justification for this is that the API is a public resource and internal refactoring should not
    // make unannounced changes to the public surface.
    public class ApiModule : NancyModule
    {
        public ApiModule(IdeastrikeContext db, IIdeaRepository ideas, IUserRepository users, IStatusRepository statuses)
            : base("/api") {
            Get["/ideas"] = _ => {
                return Response.AsJson(db.Ideas.Select(idea =>
                    new {
                        id = idea.Id,
                        title = idea.Title,
                        description = idea.Description,
                        time = SqlFunctions.DateDiff("s", new DateTime(1970, 1, 1), idea.Time),
                        author = new { id = idea.Author.Id, username = idea.Author.UserName },
                        vote_count = idea.Votes.Sum(vote => (int?)vote.Value) ?? 0,
                        status = idea.Status == null ? null : idea.Status.Title
                    }));
            };

            Post["/ideas"] = _ => {
                var model = this.Bind<EditIdeaModel>();
                if (Context.CurrentUser == null)
                    return HttpStatusCode.Unauthorized;
                var status = statuses.FindBy(d=>d.Title == "New").FirstOrDefault();

                var idea = new Idea {
                    Title = model.title,
                    Description = model.description,
                    Time = DateTime.UtcNow,
                    Author= (User)Context.CurrentUser,
                    Status = status
                };
                ideas.Add(idea);

                return HttpStatusCode.Created;  // TODO: Should return either the generated id or the json body
            };

            Put["/ideas/{id}"] = _ => {
                var model = this.Bind<EditIdeaModel>();
                if (Context.CurrentUser == null)
                    return HttpStatusCode.Unauthorized;

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

            Get["/ideas/{id}"] = _ => {
                int id = _.id;
                var o = db.Ideas.Where(idea => idea.Id == id).Select(idea =>
                    new {
                        id = idea.Id,
                        title = idea.Title,
                        description = idea.Description,
                        time = SqlFunctions.DateDiff("s", new DateTime(1970, 1, 1), idea.Time),
                        author = new { id = idea.Author.Id, username = idea.Author.UserName },
                        vote_count = idea.Votes.Sum(vote => (int?)vote.Value) ?? 0,
                        features = idea.Features.Select(feature => new { id = feature.Id, text = feature.Text, time = SqlFunctions.DateDiff("s", new DateTime(1970, 1, 1), feature.Time) }),
                        votes = idea.Votes.Select(vote => new { user = new { id = vote.UserId, username = vote.User.UserName }, value = vote.Value })
                    }).FirstOrDefault();
                if (o == null)
                    return HttpStatusCode.NotFound;
                return Response.AsJson(o);
            };

            Get["/ideas/{id}/features"] = _ => {
                int id = _.id;
                if (!db.Ideas.Any(idea => idea.Id == id))
                    return HttpStatusCode.NotFound;
                return Response.AsJson(db.Features.Where(d => d.Idea.Id == id).Select(feature =>
                    new {
                        id = feature.Id,
                        text = feature.Text,
                        time = SqlFunctions.DateDiff("s", new DateTime(1970, 1, 1), feature.Time),
                    }));
            };

            Get["/ideas/{id}/votes"] = _ => {
                int id = _.id;
                if (!db.Ideas.Any(idea => idea.Id == id))
                    return HttpStatusCode.NotFound;
                return Response.AsJson(db.Votes.Where(d => d.IdeaId == id).Select(vote =>
                    new {
                        value = vote.Value,
                        user = new { id = vote.UserId, username = vote.User.UserName }
                    }));
            };
        }
    }

    public class EditIdeaModel
    {
        public string title { get; set; }

        public string description { get; set; }
    }
}