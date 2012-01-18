using System.Collections.ObjectModel;
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
            var idea = db.Ideas.FirstOrDefault(i => i.Id == ideaid);
            if (idea == null)
                return false;

            if (idea.Activities == null)
                idea.Activities= new Collection<Activity>();

            idea.Activities.Add(activity);
            db.SaveChanges();
            return true;
        }
    }
}