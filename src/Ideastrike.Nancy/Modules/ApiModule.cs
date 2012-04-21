using System;
using System.Data.Objects.SqlClient;
using System.IO;
using System.Linq;
using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Newtonsoft.Json;

namespace Ideastrike.Nancy.Modules
{
    // The code in the API is very explicit in terms of the property names and shape of the output json
    // It does not go via the repositories, and does not simply dump models out as json
    // The justification for this is that the API is a public resource and internal refactoring should not
    // make unannounced changes to the public surface.
    public class ApiModule : NancyModule
    {
        private dynamic _settings;

        public ApiModule(IdeastrikeContext db, IIdeaRepository ideas, IUserRepository users, Settings settings)
            : base("/api") {
            
            _settings = settings;

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

            Post["/activity"] = _ =>
            {
                string content;
                using (var reader = new StreamReader(Context.Request.Body))
                {
                    content = reader.ReadToEnd();
                }
                var j = JsonConvert.DeserializeObject<dynamic>(content);
                string repourl = j.repository.url.ToString();
                string reponame = j.repository.name.ToString();
                var idea = ideas
                            .Include("Activities")
                            .Where(i => i.GithubUrl == repourl || i.GithubName == reponame)
                            .FirstOrDefault();

                if (idea == null)
                    return HttpStatusCode.NotFound;

                foreach (var c in j.commits)
                {
                    string date = c.timestamp;
                    var activity = new GitHubActivity
                    {
                        Time = DateTime.Parse(date),
                        Message = c.message,
                        CommitUrl = c.url,
                        AuthorName = c.author.name,
                        GravatarUrl = GravatarExtensions.ToGravatarUrl(c.author.email.ToString(), 40),
                        Sha = c.id
                    };

                    if (!idea.Activities.OfType<GitHubActivity>().Any(a => a.Sha == activity.Sha))
                        idea.Activities.Add(activity);
                }

                ideas.Save();
                return HttpStatusCode.Accepted;
            };
        }
    }
}