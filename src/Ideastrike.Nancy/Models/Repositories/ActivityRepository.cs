using System.Collections.Generic;
using System.Linq;
using Ideastrike.Nancy.Models.Repositories;

namespace Ideastrike.Nancy.Models
{
    public class ActivityRepository : GenericRepository<IdeastrikeContext, Activity>, IActivityRepository
    {
        public bool Add(int ideaid, Activity activity)
        {
            var idea = Context.Ideas.Find(ideaid);
            if (idea == null)
                return false;

            Context.Users.Attach(activity.User);
            Context.Ideas.Attach(idea);

            idea.Activities.Add(activity);
            Context.SaveChanges();
            return true;
        }
    }
}