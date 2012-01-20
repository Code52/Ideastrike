using System.Collections.Generic;

namespace Ideastrike.Nancy.Models
{
    public interface IActivityRepository
    {
        Activity Get(int id);
        IEnumerable<Activity> GetAll();
        bool Add(int ideaid, Activity activity);
    }
}