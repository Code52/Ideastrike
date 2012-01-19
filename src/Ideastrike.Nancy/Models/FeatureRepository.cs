using System.Collections.ObjectModel;
using System.Linq;

namespace Ideastrike.Nancy.Models
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly IdeastrikeContext db;

        public FeatureRepository(IdeastrikeContext db)
        {
            this.db = db;
        }

        public Feature Get(int id)
        {
            return db.Features.FirstOrDefault(i => i.Id == id);
        }

        public bool Add(int ideaid, Feature feature)
        {
            var idea = db.Ideas.FirstOrDefault(i => i.Id == ideaid);
            if (idea == null)
                return false;

            if (idea.Features == null)
                idea.Features = new Collection<Feature>();

            idea.Features.Add(feature);
            db.SaveChanges();
            return true;
        }
    }
}