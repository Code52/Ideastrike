using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;

namespace Ideastrike.Nancy.Models
{
    public class UsernameMapper : IUserMapper
    {
        public IUserIdentity GetUserFromIdentifier(Guid identifier)
        {
            using (var db = new IdeastrikeContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == identifier);

                return user;
            }
        }
    }
}
