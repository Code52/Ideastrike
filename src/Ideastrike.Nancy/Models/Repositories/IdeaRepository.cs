using System;
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

        public int Vote(int ideaId, Guid userId, int value)
        {
            if (Context.Votes.Any(v => v.User.Id == userId && v.IdeaId == ideaId))
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

        public int Unvote(int ideaId, Guid userId)
        {
            if (!Context.Votes.Any(v => v.User.Id == userId && v.IdeaId == ideaId))
                return Context.Ideas.Find(ideaId).Votes.Sum(v => v.Value);

            var votesToRemove = Context.Votes.Where(v => v.UserId == userId && v.IdeaId == ideaId).ToList();
            votesToRemove.ForEach(v => Context.Votes.Remove(v));
            Save();
            return Context.Ideas.Find(ideaId).Votes.Sum(v => v.Value);
        }

        public int Count
        {
            get { return Context.Ideas.Count(); }
        }
    }
}