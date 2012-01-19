using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class CommentModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;
        private readonly IActivityRepository _activities;

        public CommentModule(IIdeaRepository ideas, IActivityRepository activities)
            : base("/comment")
        {
            _ideas = ideas;
            _activities = activities;

            Post["/{idea}/add"] = parameters =>
            {
                int id = parameters.Idea;
                int userId = Request.Form.userId; // addtional validation required

                var text = Request.Form.comment;
                if (string.IsNullOrEmpty(text)) // additional validation required
                {
                    return Response.AsJson(new { result = "Error" });
                }

                var comment = new Comment
                                {
                                    UserId = userId,
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