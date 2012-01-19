using System.Collections.Generic;

namespace Ideastrike.Nancy.Models
{
    public interface IFeatureRepository
    {
        Feature GetFeature(int id);
        bool AddFeature(int ideaid, Feature feature);
    }
}