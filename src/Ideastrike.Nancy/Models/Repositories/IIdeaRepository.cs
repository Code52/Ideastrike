using System.Collections.Generic;

namespace Ideastrike.Nancy.Models.Repositories
{
    public interface IIdeaRepository : IGenericRepository<Idea>
    {
        int Vote(int ideaId, int userId, int value);
        int Count { get; }
    }
}
