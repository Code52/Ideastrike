using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;

namespace Ideastrike
{
    /// <summary>
    /// Provides a mapping between forms auth guid identifiers and
    /// real usernames
    /// </summary>
    public interface IUserMapper
    {
        /// <summary>
        /// Get the real username from an indentifier
        /// </summary>
        /// <param name="identifier">User identifier</param>
        /// <returns>Matching username, or empty</returns>
        IUserIdentity GetUserFromIdentifier(Guid identifier);
    }
}