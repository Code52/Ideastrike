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
						idea.Id,
						idea.Title,
						idea.Description,
						Time = SqlFunctions.DateDiff("s", new DateTime(1970, 1, 1), idea.Time),
						Author = new { idea.Author.Id, idea.Author.UserName },
						VoteCount = idea.Votes.Sum(vote => (int?)vote.Value) ?? 0
					}));
			};

			Get["/ideas/{id}/features"] = _ => {
				int id = _.id;
				if (!db.Ideas.Any(idea => idea.Id == id))
					return HttpStatusCode.NotFound;
				return Response.AsJson(db.Features.Where(d => d.Idea.Id == id).Select(feature =>
					new {
						feature.Id,
						feature.Text,
						Time = SqlFunctions.DateDiff("s", new DateTime(1970, 1, 1), feature.Time),
					}));
			};
		}
	}
}