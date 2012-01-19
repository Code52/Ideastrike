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

        public void Add(int ideaId, Activity activity)
        {
            var idea = db.Ideas.FirstOrDefault(i => i.Id == ideaId);
            if (idea == null)
                throw new KeyNotFoundException("Idea not found.");

            idea.Activities.Add(activity);
            db.SaveChanges();
        }

        public void Delete(int ideaId, int activityId)
        {
            var activity = db.Activities.FirstOrDefault(i => i.Id == activityId && i.IdeaId == ideaId);
            if (activity == null)
                throw new KeyNotFoundException("Activity not found.");

            db.Activities.Remove(activity);
            db.SaveChanges();
        }
    }
}