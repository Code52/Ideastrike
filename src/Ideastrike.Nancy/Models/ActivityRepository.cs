using System.Collections.Generic;
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

        public Activity Get(int id)
        {
            return db.Activities.FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<Activity> GetAll()
        {
            return db.Activities.ToList();
        }

        public bool Add(int ideaid, Activity activity)
        {
            var idea = db.Ideas.FirstOrDefault(i => i.Id == ideaid);
            if (idea == null)
                return false;

            idea.Activities.Add(activity);
            db.SaveChanges();
            return true;
        }
    }
}