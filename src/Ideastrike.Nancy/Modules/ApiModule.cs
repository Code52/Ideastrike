using System;
using System.Data.Objects.SqlClient;
using System.Linq;
using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
	public class ApiModule : NancyModule
	{
		public ApiModule(IdeastrikeContext db)
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
						Votes = idea.Votes.Select(vote => new { User = new { Id = vote.UserId, Username = "{placeholder}" }, Value = vote.Value })
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
		}
	}
}