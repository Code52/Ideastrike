using System.Collections.ObjectModel;
using System.Linq;
using Ideastrike.Nancy.Models.Repositories;

namespace Ideastrike.Nancy.Models.Repositories
{
    public class FeatureRepository : GenericRepository<IdeastrikeContext, Feature>, IFeatureRepository
    {
        public bool Add(int ideaid, Feature feature)
        {
            var idea = Context.Ideas.Find(ideaid);
            if (idea == null)
                return false;

            if (idea.Features == null)
                idea.Features = new Collection<Feature>();

            Context.Users.Attach(feature.User);
            idea.Features.Add(feature);
            Context.SaveChanges();
            return true;
        }
    }
}