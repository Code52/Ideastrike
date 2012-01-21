using System.Collections.Generic;

namespace Ideastrike.Nancy.Models
{
    public interface IIdeaRepository
    {
        IEnumerable<Idea> GetAll();
        Idea Get(int id);

        void Add(Idea idea);
        void Delete(int id);
        void Update(Idea idea);
        void Vote(Idea idea, int userId, int value);
        
        int Count { get; }
    }
}
