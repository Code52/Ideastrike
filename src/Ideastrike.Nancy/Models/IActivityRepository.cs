using System.Collections.Generic;

namespace Ideastrike.Nancy.Models
{
    public interface IActivityRepository
    {
        Activity Get(int id);
        IEnumerable<Activity> GetAll();
        void Add(int ideaId, Activity activity);
        void Delete(int ideaId, int activiyId);
    }
}