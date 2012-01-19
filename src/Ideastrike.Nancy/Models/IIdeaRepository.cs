using System.Collections.Generic;

namespace Ideastrike.Nancy.Models
{
    public interface IIdeaRepository
    {
        IEnumerable<Idea> GetAllIdeas();
        Idea GetIdea(int id);

        void AddIdea(Idea idea);
        void DeleteIdea(int id);
        void UpdateIdea(Idea idea);
        void Vote(Idea idea, int userId, int value);
        void Comment(Idea idea, int userId, string comment);
        int Count { get; }
    }
}
