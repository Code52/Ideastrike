using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
                           {
                               using (var db = new IdeastrikeContext())
                               {
                                   db.Ideas.Add(new Idea {Title = "I heard you like ideas, so I put an idea in your ideas", Time = DateTime.UtcNow});
                                   db.SaveChanges();
                                   return string.Format("Hello, world. There are {0} ideas", db.Ideas.Count());    
                               }
                               
                           };
        }
    }
}