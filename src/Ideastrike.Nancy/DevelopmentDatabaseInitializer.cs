using System;
using System.Data.Entity;
using Ideastrike.Nancy.Models;

namespace Ideastrike.Nancy
{
    public class DevelopmentDatabaseInitializer : DropCreateDatabaseIfModelChanges<IdeastrikeContext>
    {
        protected override void Seed(IdeastrikeContext context)
        {

            context.Settings.Add(new Setting {Key = "Title", Value = "Yet Another Ideastrike"});
            context.Settings.Add(new Setting {Key = "Name", Value = "Ideastrike"});
            context.Settings.Add(new Setting {Key = "WelcomeMessage", Value = "You've been.... Ideastruck"});
            context.Settings.Add(new Setting {Key = "HomePage", Value = "http://www.code52.org"});
            context.Settings.Add(new Setting {Key = "GAnalyticsKey", Value = ""});

            string ideaStatusDefault = "New";
            context.Settings.Add(new Setting {Key = "IdeaStatusChoices", Value = "New,Active,Completed,Declined"});
            context.Settings.Add(new Setting {Key = "IdeaStatusDefault", Value = ideaStatusDefault });

            context.SaveChanges();

            context.Ideas.Add(new Idea
                                  {
                                      Time = DateTime.UtcNow,
                                      Author = new User { Id = Guid.NewGuid(), UserName = "aeoth", Email = "paul@theleagueofpaul.com" },
                                      Title = "So Meta",
                                      Description = "Put an idea in your idea so you can idea when you idea",
                                      Status = ideaStatusDefault
                                  });

            context.Ideas.Add(new Idea
            {
                Time = DateTime.UtcNow,
                Author = new User { Id = Guid.NewGuid(), UserName = "shiftkey", Email = "me@brendanforster.com" },
                Title = "Lorem Ipsum",
                Description = "This is also another idea",
                Status = ideaStatusDefault
            });

            // TODO: define some more data

            context.SaveChanges();
        }

    }
}