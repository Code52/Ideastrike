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
                .Include("Activities.User")
                .Include("Features")
                .Include("Features.User")
                .Include("Author")
    			.Include("Images")
                .FirstOrDefault(i => i.Id == id);

            return idea;
        }

        public override IQueryable<Idea> GetAll()
        {
            return Context.Ideas
                .Include("Votes")
                .Include("Author");
        }

        public override void Add(Idea idea)
        {
            Context.Users.Attach(idea.Author);

            Context.Ideas.Add(idea);
            Context.SaveChanges();
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