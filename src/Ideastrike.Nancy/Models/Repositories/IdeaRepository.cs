using System.Collections.Generic;
using System.Linq;
using Ideastrike.Nancy.Models.Repositories;

namespace Ideastrike.Nancy.Models
{
    public class IdeaRepository : GenericRepository<IdeastrikeContext, Idea>, IIdeaRepository
    {
        public override Idea Get(int id)
        {
            var idea = Context.Ideas
                .Include("Votes")
                .Include("Activities")
                .Include("Features")
                .FirstOrDefault(i => i.Id == id);
            return idea;
        }

        public int Vote(int ideaId, int userId, int value)
        {
            if (Context.Votes.Any(v => v.UserId == userId && v.IdeaId == ideaId))
                return Context.Ideas.Find(ideaId).Votes.Sum(v => v.Value);

            Context.Votes.Add(new Vote
            {
                IdeaId = ideaId,
                UserId = userId,
                Value = value
            });

            Save();
            return Context.Ideas.Find(ideaId).Votes.Sum(v => v.Value);
        }

        public int Count
        {
            get { return Context.Ideas.Count(); }
        }
    }
}