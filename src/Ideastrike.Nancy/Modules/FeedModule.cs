using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class FeedModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;

        public FeedModule(IIdeaRepository ideas)
        {
            _ideas = ideas;
            Get["/feed"] = _ =>
                               {
                                   var ideasResults = _ideas.GetAll().OrderByDescending(i => i.Time).Take(50);

                                   var items = new List<SyndicationItem>();

                                   string host = UrlExtensions.ToPublicUrl(HttpContext.Current, new Uri("", UriKind.Relative));

                                   foreach (var idea in ideasResults)
                                   {
                                       var url = new Uri(string.Format("{0}{1}", host, string.Format("ideas/{0}", idea.Id)));
                                       items.Add(new SyndicationItem(idea.Title, idea.Description, url, idea.Id.ToString(), idea.Time));
                                   }

                                   var feed = new Atom10FeedFormatter(new SyndicationFeed("Code52 Ideastrike", "", new Uri(string.Format("{0}{1}", host, "feed")), items)
                                               {
                                                   Id = string.Format("{0}{1}", host, "feed"),
                                                   Links =
                                                       {
                                                           new SyndicationLink(new Uri(string.Format("{0}{1}", host, "feed")))
                                                               {
                                                                   RelationshipType = "self"
                                                               }
                                                       },
                                                   LastUpdatedTime = DateTime.Now
                                               });

                                   return Response.AsFeed(feed);
                               };
        }
    }
}

