using System;
using System.Linq;
using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Nancy.Security;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Ideastrike.Nancy.Modules
{
    public class AdminModule : NancyModule
    {
        private readonly IUserRepository _users;
        private readonly IIdeaRepository _ideas;

        public AdminModule(ISettingsRepository settings, IUserRepository users, IIdeaRepository ideas, IActivityRepository activities)
            : base("/admin")
        {
            this.RequiresAuthentication();
            this.RequiresValidatedClaims(c => c.Contains("admin"));

            _users = users;
            _ideas = ideas;

            Get["/"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.SiteTitle));
                m.Name = settings.Name;
                m.WelcomeMessage = settings.WelcomeMessage;
                m.HomePage = settings.HomePage;
                m.GAnalyticsKey = settings.GAnalyticsKey;
                return View["Admin/Index", m];
            };

            Get["/users"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.SiteTitle));
                m.Name = settings.Name;
                m.WelcomeMessage = settings.WelcomeMessage;
                m.HomePage = settings.HomePage;
                m.GAnalyticsKey = settings.GAnalyticsKey;
                m.Users = users.GetAll();
                return View["Admin/Users", m];
            };

            Get["/user/{id}"] = parameters => 
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.SiteTitle));
                Guid userIdAsGuid;
                if (!Guid.TryParse(parameters.id.ToString(), out userIdAsGuid)) 
                {
                    //not a valid guid.
                    return 404;
                }
                var user = _users.Get(userIdAsGuid);
                if (user == null) 
                {
                    //user can't be found, throw 404
                    return 404;
                }
                //give them a bigger gravatar picture...
                user.AvatarUrl = user.Email.ToGravatarUrl(180);

                m.User = user;

                return View["Admin/User", m];
            };

            Post["/user/ban"] = _ => 
            {
                var userId = Guid.Parse(Request.Form.Id);
                
                _users.Delete(userId);
                
                return Response.AsRedirect("/admin/users");
            };
            Get["/moderation"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.SiteTitle));
                //get a list of the latest ideas that are flagged as 'new'
                //this query is going to suck performance wise...
                var newIdeas = _ideas
                    .FindBy(x => x.Status == "New")
                    .OrderByDescending(x => x.Time)
                    .ToList();
                //var newIdeas = _ideas.GetAll().ToList();
                m.Ideas = newIdeas;

                return View["Admin/Moderation", m];
            };

            Get["/settings"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.SiteTitle));
                m.SiteTitle = settings.SiteTitle;
                m.Name = settings.Name;
                m.WelcomeMessage = settings.WelcomeMessage;
                m.HomePage = settings.HomePage;
                m.GAnalyticsKey = settings.GAnalyticsKey;
                m.MaxThumbnailWidth = settings.MaxThumbnailWidth;

                return View["Admin/Settings", m];
            };

            Post["/settings"] = _ =>
            {
                settings.WelcomeMessage = Request.Form.welcomemessage;
                settings.SiteTitle = Request.Form.sitetitle;
                settings.Name = Request.Form.yourname;
                settings.HomePage = Request.Form.homepage;
                settings.GAnalyticsKey = Request.Form.analyticskey;
                settings.MaxThumbnailWidth = Request.Form.maxthumbnailwidth;
               
                return Response.AsRedirect("/admin/settings");
            };

            Get["/search"] = _ => "";
            Get["/forums"] = _ => "";
            Get["/forum/{forumId}"] = _ => "";

            Get["/uservoice"] = _ => View["Admin/Uservoice", Context.Model("Admin")];
            Post["/uservoice"] = _ =>
            {
                var client = new WebClient();
                var suggestions = GetSuggestions(client, Request.Form.channel, Request.Form.forumid, Request.Form.apikey, Request.Form.trusted);

                foreach (var s in suggestions)
                {
                    string title = s.title;

                    //If the idea exists, skip it
                    if (ideas.FindBy(i => i.Title == title).Any())
                        continue;

                    string date = s.created_at;
                    var idea = new Idea
                    {
                        Title = title,
                        Description = s.text,
                        Time = DateTime.Parse(date.Substring(0, date.Length - 5)),
                    };

                    string status = string.Empty;
                    switch ((string)s.state)
                    {
                        case "approved":
                            status = "Active";
                            break;
                        case "closed" :
                            if (s.status.key == "completed")
                                status = "Completed";
                            else
                                status = "Declined";
                            break;
                        default:
                            status = "New";
                            break;
                    }
                    idea.Status = status;

                    //Get the author, or create
                    string name = s.creator.name;
                    var existing = users.FindBy(u => u.UserName == name).FirstOrDefault();
                    if (existing != null)
                        idea.Author = existing;
                    else
                    {
                        idea.Author = NewUser(s.creator);
                        users.Add(idea.Author);
                    }

                    ideas.Add(idea);

                    //Process all comments
                    var comments = GetComments(client, (string)s.id, Request.Form.channel, Request.Form.forumid, Request.Form.apikey, Request.Form.trusted);
                    List<Activity> ideaComments = new List<Activity>();
                    foreach (var c in comments)
                    {
                        string commentdate = c.created_at;
                        var comment = new Comment
                        {
                            Time = DateTime.Parse(commentdate),
                            Text = c.text
                        };

                        string commentname = c.creator.name;
                        existing = users.FindBy(u => u.UserName == commentname).FirstOrDefault();
                        if (existing != null)
                            comment.User = existing;
                        else
                        {
                            comment.User = NewUser(c.creator);
                            users.Add(comment.User);
                        }

                        activities.Add(idea.Id, comment);
                    }

                    //Process all votes
                    var votes = GetVotes(client, (string)s.id, Request.Form.channel, Request.Form.forumid, Request.Form.apikey, Request.Form.trusted);
                    foreach (var v in votes)
                    {
                        string votername = v.user.name;
                        string votesfor = v.votes_for;
                        int vote;
                        if (Int32.TryParse(votesfor, out vote))
                        {
                            existing = users.FindBy(u => u.UserName == votername).FirstOrDefault();
                            if (existing != null)
                                ideas.Vote(idea.Id, existing.Id, vote);
                            else
                            {
                                var author = NewUser(v.user);
                                users.Add(author);
                                ideas.Vote(idea.Id, author.Id, vote);
                            }
                        }
                    }
                }

                return Response.AsRedirect("/admin");
            };
        }

        private static User NewUser(dynamic user)
        {
            var author = new User
            {
                Id = Guid.NewGuid(),
                UserName = user.name,
            };

            if (user.avatar_url != null)
            {
                string avatar = user.avatar_url;
                if (avatar.Contains("&"))
                    avatar = avatar.Substring(0, avatar.IndexOf("&"));
                author.AvatarUrl = avatar;
            }
            return author;
        }

        private static IEnumerable<dynamic> GetSuggestions(WebClient client, string SuggestionsChannel, string ForumID, string APIKey, bool trusted)
        {
            var url = new Uri(string.Format("http://{0}.uservoice.com/api/v1/forums/{1}/suggestions.json?per_page=100&sort=newest&client={2}{3}", SuggestionsChannel, ForumID, APIKey, (trusted) ? "&filter=all" : ""));
            var uvResponse = client.DownloadString(url);
            var suggestions = JsonConvert.DeserializeObject<dynamic>(uvResponse).suggestions;

            return suggestions;
        }

        private static IEnumerable<dynamic> GetComments(WebClient client, string IdeadId, string SuggestionsChannel, string ForumID, string APIKey, bool trusted)
        {
            var uvResponse = client.DownloadString(new Uri(string.Format("http://{0}.uservoice.com/api/v1/forums/{1}/suggestions/{3}/comments.json?per_page=50&client={2}{4}", SuggestionsChannel, ForumID, APIKey, IdeadId, (trusted) ? "&filter=all" : "")));
            var comments = JsonConvert.DeserializeObject<dynamic>(uvResponse).comments;

            return comments;
        }

        private static IEnumerable<dynamic> GetVotes(WebClient client, string IdeadId, string SuggestionsChannel, string ForumID, string APIKey, bool trusted)
        {
            var uvResponse = client.DownloadString(new Uri(string.Format("http://{0}.uservoice.com/api/v1/forums/{1}/suggestions/{3}/supporters.json?per_page=50&client={2}{4}", SuggestionsChannel, ForumID, APIKey, IdeadId, (trusted) ? "&filter=all" : "")));
            var supporters = JsonConvert.DeserializeObject<dynamic>(uvResponse).supporters;

            return supporters;
        }
    }
}