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

        public IEnumerable<Idea> GetAllIdeas()
        {
            return (db.Ideas.ToList());
        }

        public void AddIdea(Idea idea)
        {
            db.Ideas.Add(idea);
            db.SaveChanges();
        }

        public void DeleteIdea(int id)
        {
            var idea = db.Ideas.FirstOrDefault(i => i.Id == id);
            db.Ideas.Remove(idea);
            db.SaveChanges();

        }

        public Idea GetIdea(int id)
        {
            return db.Ideas
                .Include("Activities")
                .Include("Votes")
                .FirstOrDefault(i => i.Id == id);
        }

        public void UpdateIdea(Idea idea)
        {
            var tmpIdea = db.Ideas.Single(i => i.Id == idea.Id);
            tmpIdea = idea;
            db.SaveChanges();
        }

        public void Vote(Idea idea, int userId, int value)
        {
            idea.Votes.Add(new Vote
                               {
                                   IdeaId = idea.Id,
                                   UserId = userId,
                                   Value = value
                               });

            UpdateIdea(idea);
        }

        public int Count
        {
            get
            {
                {
                    return db.Ideas.Count();
                }
            }
        }
    }
}