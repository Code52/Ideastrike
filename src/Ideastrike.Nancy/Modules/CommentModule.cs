using System;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class CommentModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;
        private readonly IActivityRepository _activities;

        public CommentModule(IIdeaRepository ideas, IActivityRepository activities)
            : base("/idea")
        {
            _ideas = ideas;
            _activities = activities;

            Post["/{idea}/comment"] = _ =>
            {
                int id = _.Idea;
                var comment = new Comment
                                {
                                    Time = DateTime.UtcNow,
                                    Text = Request.Form.comment
                                };
                _activities.Add(id, comment);

                return Response.AsRedirect(string.Format("/idea/{0}#{1}", id, comment.Id));
            };

            Get["/{idea}/comment/{id}/delete"] = parameters =>
            {
                int comment = parameters.id;
                int idea = parameters.idea;
                _activities.Delete(idea, comment);
                return string.Format("Deleted Comment {0} for Idea {1}", comment, idea);
            };
        }
    }
}