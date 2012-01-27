using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Ideastrike.Nancy.Models.Repositories;
using Nancy.Security;

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


        public IQueryable<User> FindBy(System.Linq.Expressions.Expression<Func<User, bool>> predicate)
        {
            IQueryable<User> query = Context.Set<User>()
                                            .Include("UserClaims")
                                            .Include("UserClaims.Claim")
                                            .Where(predicate);
            return query;
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

        public void AddRole(User user, string roleName)
        {
            var c = Context.Claims.FirstOrDefault(claim => claim.Name.ToLower() == roleName.ToLower());
            if (c == null)
            {
                Context.Claims.Add(new Claim {Name = roleName});
                Context.SaveChanges();
            }
            if (user.UserClaims == null)
                user.UserClaims = new Collection<UserClaim>();

            user.UserClaims.Add(new UserClaim { Claim = c });
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
