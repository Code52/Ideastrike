using Ideastrike.Nancy.App_Start;
using Ideastrike.Nancy.Models;

namespace Ideastrike.Nancy
{
    public class ProductionDatabaseInitializer : AppHarborDatabaseInitializer<IdeastrikeContext>
    {

        public override void SeedData(IdeastrikeContext context)
        {
            context.Settings.Add(new Setting { Key = "Title", Value = "Yet Another Ideastrike" });
            context.Settings.Add(new Setting { Key = "Name", Value = "Ideastrike" });
            context.Settings.Add(new Setting { Key = "WelcomeMessage", Value = "You've been.... Ideastruck" });
            context.Settings.Add(new Setting { Key = "HomePage", Value = "http://www.code52.org" });
            context.Settings.Add(new Setting { Key = "GAnalyticsKey", Value = "" });

            string ideaStatusDefault = "New";
            context.Settings.Add(new Setting { Key = "IdeaStatusChoices", Value = "New,Active,Completed,Declined" });
            context.Settings.Add(new Setting { Key = "IdeaStatusDefault", Value = ideaStatusDefault });

            // TODO: define some more data

            context.SaveChanges();
        }
    }
}