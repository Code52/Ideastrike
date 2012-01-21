using System.Collections.Generic;

namespace Ideastrike.Nancy.Models.Repositories
{
    public interface IActivityRepository : IGenericRepository<Activity>
    {
        bool Add(int ideaid, Activity activity);
    }
}