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

            context.Statuses.Add(new Status { Title = "New" });
            context.Statuses.Add(new Status { Title = "Active" });
            context.Statuses.Add(new Status { Title = "Completed" });
            context.Statuses.Add(new Status { Title = "Declined" });

            // TODO: define some more data

            context.SaveChanges();
        }
    }
}