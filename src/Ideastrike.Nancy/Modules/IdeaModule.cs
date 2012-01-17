using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaModule : NancyModule
    {
        public IdeaModule()
        {
            Get["/idea/{id}"] = parameters =>
            {
                using (var db = new IdeastrikeContext())
                {
                    double id = parameters.id;
                    var idea = db.Ideas.Where(i => i.Id == id).FirstOrDefault();
                    return string.Format("Id:{0} Title:{1} Description:{2}", idea.Id, idea.Title, idea.Description);
                }
            };

            Get["/idea/{id}/delete"] = parameters =>
            {
                using (var db = new IdeastrikeContext())
                {
                    double id = parameters.id;
                    var idea = db.Ideas.Where(i => i.Id == id).FirstOrDefault();
                    db.Ideas.Remove(idea);
                    db.SaveChanges();
                    return string.Format("Deleted Item {0}", id);
                }
            };
        }
    }
}