using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ideastrike.Nancy.Models
{
    public interface IIdeaRepository
    {
        IEnumerable<Idea> GetAllIdeas();
        Idea GetIdea(double id);

        void AddIdea(Idea idea);
        void DeleteIdea(double IdeaId);         
        void UpdateIdea(Idea idea);

        int CountIdeas();
    }
}
