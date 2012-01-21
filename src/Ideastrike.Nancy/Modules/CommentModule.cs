using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Nancy;
using Ideastrike.Nancy.Models.Repositories;
using Nancy.Security;

namespace Ideastrike.Nancy.Modules
{
    public class CommentModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;
        private readonly IActivityRepository _activities;
        private readonly IUserRepository _users;

        public CommentModule(IIdeaRepository ideas, IActivityRepository activities, IUserRepository users)
            : base("/comment")
        {
            this.RequiresAuthentication();

            _ideas = ideas;
            _activities = activities;
            _users = users;

            Post["/{idea}/add"] = parameters =>
            {
                int id = parameters.Idea;

                var user = _users.FindBy(u => u.UserName == Context.CurrentUser.UserName).FirstOrDefault();

                if (user == null) return Response.AsRedirect(string.Format("/idea/{0}", id)); //TODO: Problem looking up the user? Return an error


                var text = Request.Form.comment;
                if (string.IsNullOrEmpty(text)) // additional validation required
                {
                    return Response.AsJson(new { result = "Error" });
                }

                var comment = new Comment
                                {
                                    User = user,
                                    Time = DateTime.UtcNow,
                                    Text = Request.Form.comment
                                };

                _activities.Add(id, comment);

                // why not return JSON here and leave it up to client to render inline?
                return Response.AsRedirect(string.Format("/idea/{0}#{1}", id, comment.Id));
            };

            // TODO: shouldn't these actually sit under the Comment root
            // TODO: user should be able to edit their own comment
            Post["/{id}/edit"] = parameters => Response.AsJson(new { result = "Error" });

            // TODO: user should be able to remove their own comment
            Post["/{id}/delete"] = parameters => Response.AsJson(new { result = "Error" });
        }
    }
}