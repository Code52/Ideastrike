using System;
using System.Data.Entity.Migrations;
using Ideastrike.Nancy.Models;

namespace Ideastrike.Nancy.Migrations
{
    internal sealed class IdeastrikeDbConfiguration : DbMigrationsConfiguration<IdeastrikeContext>
    {
        private const string IdeaStatusDefault = "New";

        public IdeastrikeDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(IdeastrikeContext context)
        {
            context.Settings.AddOrUpdate(s => s.Key, new Setting { Key = "Title", Value = "Yet Another Ideastrike" });
            context.Settings.AddOrUpdate(s => s.Key, new Setting { Key = "Name", Value = "Ideastrike" });
            context.Settings.AddOrUpdate(s => s.Key, new Setting { Key = "WelcomeMessage", Value = "You've been.... Ideastruck" });
            context.Settings.AddOrUpdate(s => s.Key, new Setting { Key = "HomePage", Value = "http://www.code52.org" });
            context.Settings.AddOrUpdate(s => s.Key, new Setting { Key = "GAnalyticsKey", Value = "" });
            context.Settings.AddOrUpdate(s => s.Key, new Setting { Key = "IdeaStatusChoices", Value = "New,Active,Completed,Declined" });
            context.Settings.AddOrUpdate(s => s.Key, new Setting { Key = "IdeaStatusDefault", Value = IdeaStatusDefault });
            context.SaveChanges();

#if DEBUG
            context.Ideas.Add(new Idea
                                  {
                                      Time = DateTime.UtcNow,
                                      Author = new User { Id = Guid.NewGuid(), UserName = "aeoth", Email = "paul@theleagueofpaul.com" },
                                      Title = "So Meta",
                                      Description = "Put an idea in your idea so you can idea when you idea",
                                      Status = IdeaStatusDefault
                                  });

            context.Ideas.Add(new Idea
            {
                Time = DateTime.UtcNow,
                Author = new User { Id = Guid.NewGuid(), UserName = "shiftkey", Email = "me@brendanforster.com" },
                Title = "Lorem Ipsum",
                Description = "This is also another idea",
                Status = IdeaStatusDefault
            });
            context.SaveChanges();
#endif
        }
    }
}
