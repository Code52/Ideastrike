using System.Data.Entity;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Ideastrike.Nancy.App_Start.DontDropDbJustCreateTablesIfModelChangedStart), "Start")]

namespace Ideastrike.Nancy.App_Start
{
    public static class DontDropDbJustCreateTablesIfModelChangedStart
    {
        public static void Start()
        {
#if DEBUG
            Database.SetInitializer(new DevelopmentDatabaseInitializer()); 
#else
            Database.SetInitializer(new DatabaseInitializer());
#endif
        }
    }
}
