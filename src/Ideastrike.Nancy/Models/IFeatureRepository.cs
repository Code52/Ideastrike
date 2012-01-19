namespace Ideastrike.Nancy.Models
{
    public interface IFeatureRepository
    {
        Feature Get(int id);
        void Add(int ideaId, Feature feature);
    }
}