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

        public IEnumerable<Idea> GetAll()
        {
            return (db.Ideas.ToList());
        }

        public void Add(Idea idea)
        {
            db.Ideas.Add(idea);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var idea = db.Ideas.FirstOrDefault(i => i.Id == id);
            db.Ideas.Remove(idea);
            db.SaveChanges();

        }

        public Idea Get(int id)
        {
            return db.Ideas
                .Include("Activities")
                .Include("Votes")
                .FirstOrDefault(i => i.Id == id);
        }

        public void Update(Idea idea)
        {
            var tmpIdea = db.Ideas.Single(i => i.Id == idea.Id);
            tmpIdea = idea; // wha?
            db.SaveChanges();
        }

        public void Vote(Idea idea, int userId, int value)
        {
            if (db.Votes.Any(v => v.UserId == userId && v.IdeaId == idea.Id))
                return;

            idea.Votes.Add(new Vote
                               {
                                   IdeaId = idea.Id,
                                   UserId = userId,
                                   Value = value
                               });

            Update(idea);
        }

        public int Count
        {
            get { return db.Ideas.Count(); }
        }
    }
}
