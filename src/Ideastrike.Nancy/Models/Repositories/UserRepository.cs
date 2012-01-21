using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;

namespace Ideastrike.Nancy.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly IdeastrikeContext db;

        public UserRepository(IdeastrikeContext db)
        {
            this.db = db;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == identifier);

            return user;
        }

        public User GetUserFromUserIdentity(string identity)
        {
            return db.Users.FirstOrDefault(u => u.Identity == identity);
        }

        public IEnumerable<User> GetAll()
        {
            return (db.Users.ToList());
        }

        public void Add(User users)
        {
            db.Users.Add(users);
            db.SaveChanges();
        }

        public IEnumerable<User> GetActive()
        {
            return db.Users.Where(u => u.IsActive).ToList();
        }
}
}
