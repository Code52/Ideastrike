using System.Collections.Generic;
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

        public void Add(int ideaId, Feature feature)
        {
            var idea = db.Ideas.FirstOrDefault(i => i.Id == ideaId);
            if (idea == null)
                throw new KeyNotFoundException("Idea not found.");

            idea.Features.Add(feature);
            db.SaveChanges();
        }
    }
}