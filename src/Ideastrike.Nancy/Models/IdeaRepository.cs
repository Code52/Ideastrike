using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ideastrike.Nancy.Models
{
    public class IdeaRepository : IIdeaRepository
    {
        private IdeastrikeContext db = new IdeastrikeContext();

        IEnumerable<Idea> IIdeaRepository.GetAllIdeas()
        {
            return (db.Ideas.ToList());
        }

        void IIdeaRepository.AddIdea(Idea idea)
        {
            db.Ideas.Add(idea);
            db.SaveChanges();
        }

        void IIdeaRepository.DeleteIdea(double IdeaId)
        {
            double id = IdeaId;
            var idea = db.Ideas.FirstOrDefault(i => i.Id == id);
            db.Ideas.Remove(idea);
            db.SaveChanges();
           
        }

        Idea IIdeaRepository.GetIdea(double id)
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