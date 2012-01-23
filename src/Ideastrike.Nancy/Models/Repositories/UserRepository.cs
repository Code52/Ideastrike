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
    public class UserRepository : IUserRepository
    {
        public UserRepository(IdeastrikeContext db)
        {
            _entities = db;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier)
        {
            var user = FindBy(u => u.Id == identifier).FirstOrDefault();

            return user;
        }

        public User GetUserFromUserIdentity(string identity)
        {
            return FindBy(u => u.Identity == identity).FirstOrDefault();
        }

        private IdeastrikeContext _entities;
        public IdeastrikeContext Context
        {

            get { return _entities; }
            set { _entities = value; }
        }

        public virtual IQueryable<User> GetAll()
        {

            IQueryable<User> query = _entities.Set<User>();
            return query;
        }

        public virtual User Get(Guid id)
        {
            var query = _entities.Set<User>().Find(id);
            return query;
        }

        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate)
        {
            IQueryable<User> query = _entities.Set<User>().Where(predicate);
            return query;
        }

        public virtual void Add(User entity)
        {
            _entities.Set<User>().Add(entity);
            _entities.SaveChanges();
        }

        public virtual void Delete(Guid id)
        {
            var entity = Get(id);
            entity.IsActive = false;
            entity.Identity = "Deleted User - " + entity.Identity;
            _entities.SaveChanges();
        }

        public virtual void Edit(User entity)
        {
            _entities.Entry(entity).State = System.Data.EntityState.Modified;
            _entities.SaveChanges();
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
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
