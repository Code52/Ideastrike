namespace Ideastrike.Nancy.Models
{
    public interface IFeatureRepository
    {
        Feature Get(int id);
        bool Add(int ideaid, Feature feature);
    }
}