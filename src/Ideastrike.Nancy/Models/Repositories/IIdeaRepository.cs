using System;

namespace Ideastrike.Nancy.Models.Repositories
{
    public interface IIdeaRepository : IGenericRepository<Idea>
    {
        int Vote(int ideaId, Guid userId, int value);
        int Unvote(int ideaId, Guid userId);
        int Count { get; }
    }
}
