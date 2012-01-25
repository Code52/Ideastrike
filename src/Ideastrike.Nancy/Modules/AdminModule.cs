using System;
using System.Dynamic;
using System.Linq;
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
        public AdminModule(IdeastrikeContext dbContext, ISettingsRepository settings, IUserRepository users, IIdeaRepository ideas, IActivityRepository activities)
            : base("/admin")
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.Title));
                m.Name = settings.Name;
                m.WelcomeMessage = settings.WelcomeMessage;
                m.HomePage = settings.HomePage;
                m.GAnalyticsKey = settings.GAnalyticsKey;
                return View["Admin/Index", m];
            };

            Get["/users"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.Title));
                m.Name = settings.Name;
                m.WelcomeMessage = settings.WelcomeMessage;
                m.HomePage = settings.HomePage;
                m.GAnalyticsKey = settings.GAnalyticsKey;
                m.Users = users.GetAll();
                return View["Admin/Users", m];
            };

            Get["/moderation"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.Title));
                m.Name = settings.Name;
                m.WelcomeMessage = settings.WelcomeMessage;
                m.HomePage = settings.HomePage;
                m.GAnalyticsKey = settings.GAnalyticsKey;
                return View["Admin/Moderation", m];
            };

            Get["/settings"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.Title));
                m.Name = settings.Name;
                m.WelcomeMessage = settings.WelcomeMessage;
                m.HomePage = settings.HomePage;
                m.GAnalyticsKey = settings.GAnalyticsKey;

                return View["Admin/Settings", m];
            };

            Post["/settings"] = _ =>
            {
                settings.WelcomeMessage = Request.Form.welcomemessage;
                settings.Title = Request.Form.title;
                settings.Name = Request.Form.yourname;
                settings.HomePage = Request.Form.homepage;
                settings.GAnalyticsKey = Request.Form.analyticskey;
                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    var y = ex.Message;
                }

                return Response.AsRedirect("/admin/settings");
            };

            Get["/search"] = _ => "";
            Get["/forums"] = _ => "";
            Get["/forum/{forumId}"] = _ => "";

            Get["/uservoice"] = _ => View["Admin/Uservoice", Context.Model("Admin")];
            Post["/uservoice"] = _ =>
            {
                var client = new WebClient();
                var suggestions = GetSuggestions(client, Request.Form.channel, Request.Form.forumid, Request.Form.apikey);

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

                    //Get the author, or create
                    string name = s.creator.name;
                    var existing = users.FindBy(u => u.UserName == name).FirstOrDefault();
                    if (existing != null)
                        idea.Author = existing;
                    else
                    {
                        idea.Author = NewUser(name);
                        users.Add(idea.Author);
                    }

                    ideas.Add(idea);

                    //Process all comments
                    var comments = GetComments(client, (string)s.id, Request.Form.channel, Request.Form.forumid, Request.Form.apikey);
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
                            comment.User = NewUser(commentname);
                            users.Add(comment.User);
                        }

                        activities.Add(idea.Id, comment);
                    }

                    //Process all votes
                    var votes = GetVotes(client, (string)s.id, Request.Form.channel, Request.Form.forumid, Request.Form.apikey);
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
                                var author = NewUser(votername);
                                users.Add(author);
                                ideas.Vote(idea.Id, author.Id, vote);
                            }
                        }
                    }
                }

                return Response.AsRedirect("/admin");
            };
        }

        private static User NewUser(string Username)
        {
            var author = new User
            {
                Id = Guid.NewGuid(),
                UserName = Username,
            };   

            return author;
        }

        private static IEnumerable<dynamic> GetSuggestions(WebClient client, string SuggestionsChannel, string ForumID, string APIKey)
        {
            var uvResponse = client.DownloadString(new Uri(string.Format("http://{0}.uservoice.com/api/v1/forums/{1}/suggestions.json?per_page=50&sort=newest&client={2}", SuggestionsChannel, ForumID, APIKey)));
            var suggestions = JsonConvert.DeserializeObject<dynamic>(uvResponse).suggestions;

            return suggestions;
        }

        private static IEnumerable<dynamic> GetComments(WebClient client, string IdeadId, string SuggestionsChannel, string ForumID, string APIKey)
        {
            var uvResponse = client.DownloadString(new Uri(string.Format("http://{0}.uservoice.com/api/v1/forums/{1}/suggestions/{3}/comments.json?client={2}", SuggestionsChannel, ForumID, APIKey, IdeadId)));
            var comments = JsonConvert.DeserializeObject<dynamic>(uvResponse).comments;

            return comments;
        }

        private static IEnumerable<dynamic> GetVotes(WebClient client, string IdeadId, string SuggestionsChannel, string ForumID, string APIKey)
        {
            var uvResponse = client.DownloadString(new Uri(string.Format("http://{0}.uservoice.com/api/v1/forums/{1}/suggestions/{3}/supporters.json?client={2}", SuggestionsChannel, ForumID, APIKey, IdeadId)));
            var supporters = JsonConvert.DeserializeObject<dynamic>(uvResponse).supporters;

            return supporters;
        }
    }
}