using System.Collections.Generic;
using System.Linq;

namespace Ideastrike.Nancy.Models
{
    public class IdeaRepository : IIdeaRepository
    {
        private readonly IdeastrikeContext db;

        public IdeaRepository(IdeastrikeContext db)
        {
            this.db = db;
        }

        IEnumerable<Idea> IIdeaRepository.GetAllIdeas()
        {
            return (db.Ideas.ToList());
        }

        void IIdeaRepository.AddIdea(Idea idea)
        {
            db.Ideas.Add(idea);
            db.SaveChanges();
        }

        void IIdeaRepository.DeleteIdea(int id)
        {
            var idea = db.Ideas.FirstOrDefault(i => i.Id == id);
            db.Ideas.Remove(idea);
            db.SaveChanges();
           
        }

        Idea IIdeaRepository.GetIdea(int id)
        {
            return db.Ideas.FirstOrDefault(i => i.Id == id);
        }                    

        void IIdeaRepository.UpdateIdea(Idea idea)
        {
            var tmpIdea = db.Ideas.Single(i => i.Id == idea.Id);
            tmpIdea = idea;
            db.SaveChanges();
        }

        int IIdeaRepository.CountIdeas()
        {
            return db.Ideas.Count();
        }
    }
}