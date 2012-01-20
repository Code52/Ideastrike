using System;
using System.Data.Entity;
using Ideastrike.Nancy.Models;

namespace Ideastrike.Nancy
{
    public class DevelopmentDatabaseInitializer : DropCreateDatabaseIfModelChanges<IdeastrikeContext>
    {
        protected override void Seed(IdeastrikeContext context)
        {
            context.Ideas.Add(new Idea
                                  {
                                      Time = DateTime.Now,
                                      Author = new User { Id = 1, Username = "aeoth" },
                                      Title = "So Meta",
                                      Description = "Put an idea in your idea so you can idea when you idea",
                                  });

            context.Ideas.Add(new Idea
            {
                Time = DateTime.Now,
                Author = new User { Id = 2, Username = "shiftkey" },
                Title = "Lorem Ipsum",
                Description = "This is also another idea",
            });

            context.Settings.Add(new Setting { Key = "Title", Value = "Yet Another Ideastrike" });
            context.Settings.Add(new Setting { Key = "Name", Value = "Ideastrike" });
            context.Settings.Add(new Setting { Key = "WelcomeMessage", Value = "You've been.... Ideastruck"});
            context.Settings.Add(new Setting { Key = "HomePage", Value = "http://www.code52.org" });
            context.Settings.Add(new Setting { Key = "GAnalyticsKey", Value = "" });

            // TODO: define some more data

            context.SaveChanges();
        }

    }
}