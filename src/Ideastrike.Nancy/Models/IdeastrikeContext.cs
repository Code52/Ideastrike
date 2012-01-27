﻿using System.Data.Entity;

namespace Ideastrike.Nancy.Models
{
    public class IdeastrikeContext : DbContext
    {
        public IdeastrikeContext(): base("Ideastrike")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}