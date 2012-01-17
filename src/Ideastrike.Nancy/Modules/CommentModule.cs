using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class CommentModule : NancyModule
    {
        public CommentModule() : base ("/idea")
        {
            Get["/{idea}/comment"] = parameters =>
            {
                using (var db = new IdeastrikeContext())
                {
                    db.Comments.Add(new Comment {IdeaId = parameters.idea });
                    db.SaveChanges();
                    return View["Comment/Index", string.Format("Hello, world. There are {0} comments for Idea {1}", db.Comments.Count(), parameters.idea)];
                }
            };

            Get["/{idea}/comment/{id}"] = parameters =>
            {
                using (var db = new IdeastrikeContext())
                {
                    int id = parameters.id;
                    int idea = parameters.idea;
                    var comment = db.Comments.Where(i => i.Id == id && i.IdeaId == idea).FirstOrDefault();
                    return string.Format("Comment Id:{0}", comment.Id);
                }
            };

            Get["/{idea}/comment/{id}/delete"] = parameters =>
            {
                using (var db = new IdeastrikeContext())
                {
                    int id = parameters.id;
                    int idea = parameters.idea;
                    
                    var comment = db.Comments.Where(i => i.Id == id && i.IdeaId == idea).FirstOrDefault();
                    db.Comments.Remove(comment);
                    db.SaveChanges();
                    return string.Format("Deleted Comment {0} for Idea {1}", id, idea);
                }
            };
        }
    }
}