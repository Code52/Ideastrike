using System.Collections.Generic;

namespace Ideastrike.Nancy.Models
{
    public interface IActivityRepository
    {
        Activity GetActivity(int id);
        bool AddActivity(int ideaid, Activity activity);
    }
}