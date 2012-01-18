using System.Data.Entity;

namespace Ideastrike.Nancy.Models
{
    public class IdeastrikeContext : DbContext
    {
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<VotesToUser> VotesToUsers { get; set; }
    }
}