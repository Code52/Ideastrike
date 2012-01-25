using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Web;
using Ideastrike.Nancy.Models.Repositories;
using Nancy.Security;
using System.Data.Entity;

namespace Ideastrike.Nancy.Models
{
    public class UserRepository : GenericRepository<IdeastrikeContext, User>, IUserRepository
    {
        public UserRepository(IdeastrikeContext ctx) : base(ctx) { }

        public IUserIdentity GetUserFromIdentifier(Guid identifier)
        {
            var user = FindBy(u => u.Id == identifier).FirstOrDefault();

            return user;
        }

        public User GetUserFromUserIdentity(string identity)
        {
            return FindBy(u => u.Identity == identity).FirstOrDefault();
        }

        public virtual User Get(Guid id)
        {
            var query = Context.Set<User>().Find(id);
            return query;
        }

        public virtual void Delete(Guid id)
        {
            var entity = Get(id);
            entity.IsActive = false;
            entity.Identity = "Deleted User - " + entity.Identity;
            Context.SaveChanges();
        }

        public ICollection<Vote> GetVotes(Guid id)
        {
            var user = Context.Users
                           .Include("Votes")
                           .Include("Votes.Idea")
                           .FirstOrDefault(u => u.Id == id);

            return user.Votes;
        }
    }
}
