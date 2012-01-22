using System.Data.Entity;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Ideastrike.Nancy.App_Start.DatabaseSelector), "Start")]

namespace Ideastrike.Nancy.App_Start
{
    public static class DatabaseSelector
    {
        public static void Start()
        {
#if DEBUG
            Database.SetInitializer(new DevelopmentDatabaseInitializer()); 
#else
            Database.SetInitializer(new ProductionDatabaseInitializer());
#endif
        }
    }
}
