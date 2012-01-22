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
        public ApiModule(IdeastrikeContext db, IIdeaRepository ideas)
            : base("/api") {
            Get["/ideas"] = _ => {
                return Response.AsJson(db.Ideas.Select(idea =>
                    new {
                        Id = idea.Id,
                        Title = idea.Title,
                        Description = idea.Description,
                        Time = SqlFunctions.DateDiff("s", new DateTime(1970, 1, 1), idea.Time),
                        Author = new { Id = idea.Author.Id, Username = idea.Author.Username },
                        VoteCount = idea.Votes.Sum(vote => (int?)vote.Value) ?? 0
                    }));
            };

            Post["/ideas"] = _ => {
                var model = this.Bind<NewIdeaModel>();
                var idea = new Idea {
                    Title= model.Title,
                    Description= model.Description,
                    Time = DateTime.UtcNow
                };
                ideas.Add(idea);

                return HttpStatusCode.Created;  // TODO: Should return either the generated id or the json body
            };

            Get["/ideas/{id}"] = _ => {
                int id = _.id;
                var o = db.Ideas.Where(idea => idea.Id == id).Select(idea =>
                    new {
                        Id = idea.Id,
                        Title = idea.Title,
                        Description = idea.Description,
                        Time = SqlFunctions.DateDiff("s", new DateTime(1970, 1, 1), idea.Time),
                        Author = new { Id = idea.Author.Id, Username = idea.Author.Username },
                        VoteCount = idea.Votes.Sum(vote => (int?)vote.Value) ?? 0,
                        Features = idea.Features.Select(feature => new { Id = feature.Id, Text = feature.Text, Time = SqlFunctions.DateDiff("s", new DateTime(1970, 1, 1), feature.Time) }),
                        Votes = idea.Votes.Select(vote => new { User = new { Id = vote.UserId, Username = vote.User.Username }, Value = vote.Value })
                    }).FirstOrDefault();
                if (o == null)
                    return HttpStatusCode.NotFound;
                return Response.AsJson(o);
            };

            Get["/ideas/{id}/features"] = _ => {
                int id = _.id;
                if (!db.Ideas.Any(idea => idea.Id == id))
                    return HttpStatusCode.NotFound;
                return Response.AsJson(db.Features.Where(d => d.IdeaId == id).Select(feature =>
                    new {
                        Id = feature.Id,
                        Text = feature.Text,
                        Time = SqlFunctions.DateDiff("s", new DateTime(1970, 1, 1), feature.Time),
                    }));
            };

            Get["/ideas/{id}/votes"] = _ => {
                int id = _.id;
                if (!db.Ideas.Any(idea => idea.Id == id))
                    return HttpStatusCode.NotFound;
                return Response.AsJson(db.Votes.Where(d => d.IdeaId == id).Select(vote =>
                    new {
                        Value = vote.Value,
                        User = new { Id = vote.UserId, Username = vote.User.Username }
                    }));
            };
        }
    }

    public class NewIdeaModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}