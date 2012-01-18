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
                _activities.AddActivity(id, comment);

                return Response.AsRedirect(string.Format("/idea/{0}#{1}", id, comment.Id));
            };

            /*
            Get["/{idea}/comment/{id}"] = parameters =>
            {
                using (var db = new IdeastrikeContext())
                {
                    int id = parameters.id;
                    int idea = parameters.idea;
                    var comment = db.Comments.FirstOrDefault(i => i.Id == id && i.IdeaId == idea);
                    return string.Format("Comment Id:{0}", comment.Id);
                }
            };

            Get["/{idea}/comment/{id}/delete"] = parameters =>
            {
                using (var db = new IdeastrikeContext())
                {
                    int id = parameters.id;
                    int idea = parameters.idea;
                    
                    var comment = db.Comments.FirstOrDefault(i => i.Id == id && i.IdeaId == idea);
                    db.Comments.Remove(comment);
                    db.SaveChanges();
                    return string.Format("Deleted Comment {0} for Idea {1}", id, idea);
                }
            };*/
        }
    }
}