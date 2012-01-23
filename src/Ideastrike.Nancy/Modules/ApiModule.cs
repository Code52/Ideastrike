using System;
using System.Data.Objects.SqlClient;
using System.Linq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    // The code in the API is very explicit in terms of the property names and shape of the output json
    // It does not go via the repositories, and does not simply dump models out as json
    // The justification for this is that the API is a public resource and internal refactoring should not
    // make unannounced changes to the public surface.
    public class ApiModule : NancyModule
    {
        public ApiModule(IdeastrikeContext db, IIdeaRepository ideas, IUserRepository users, ISettingsRepository settings)
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
                        status = idea.Status
                    }));
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
}