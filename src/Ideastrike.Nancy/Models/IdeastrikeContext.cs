using System.Configuration;
using System.Data.Entity;

namespace Ideastrike.Nancy.Models
{
    public class IdeastrikeContext : DbContext
    {
        public IdeastrikeContext()
        {
            Configuration.ProxyCreationEnabled = false;

            var apphb = ConfigurationManager.ConnectionStrings["SQLSERVER_CONNECTION_STRING"];

            if (apphb != null)
            {
                Database.Connection.ConnectionString = apphb.ConnectionString;
            }
        }

        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Status> Statuses { get; set; }
    }
}