using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ideastrike.Nancy.Models;
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

        IEnumerable<User> GetAll();

        void Add(User users);
    }
}