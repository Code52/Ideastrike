using Ideastrike.Nancy.Models.Repositories;
namespace Ideastrike.Nancy.Models
{
    public interface IFeatureRepository : IGenericRepository<Feature>
    {
        bool Add(int ideaid, Feature feature);
    }
}