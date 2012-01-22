using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nancy.Security;

namespace Ideastrike.Nancy.Models
{
    /// <summary>
    /// Provides a mapping between forms auth guid identifiers and
    /// real usernames
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get the real username from an indentifier
        /// </summary>
        /// <param name="identifier">User identifier</param>
        /// <returns>Matching username, or empty</returns>
        IUserIdentity GetUserFromIdentifier(Guid identifier);

        User GetUserFromUserIdentity(string identity);

        IQueryable<User> GetAll();
        IQueryable<User> FindBy(Expression<Func<User, bool>> predicate);
        User Get(Guid id);
        void Add(User entity);
        void Delete(Guid id);
        void Edit(User entity);
        void Save();

        ICollection<Vote> GetVotes(Guid id);
    }
}