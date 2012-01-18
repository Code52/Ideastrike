using System.Linq;

namespace Ideastrike.Nancy.Models
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly IdeastrikeContext db;

        public ActivityRepository(IdeastrikeContext db)
        {
            this.db = db;
        }

        public Activity GetActivity(int id)
        {
            return db.Activities.FirstOrDefault(i => i.Id == id);
        }

        public bool AddActivity(int ideaid, Activity activity)
        {
            var firstOrDefault = db.Ideas.FirstOrDefault(i => i.Id == ideaid);
            if (firstOrDefault == null)
                return false;

            firstOrDefault.Activities.Add(activity);
            db.SaveChanges();
            return true;
        }
    }
}