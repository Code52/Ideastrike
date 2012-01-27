using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nancy;
using Nancy.Security;

namespace Ideastrike.Nancy.Models
{
    public class User : IUserIdentity
    {
        public User()
        {
            UserClaims = new Collection<UserClaim>();
        }

        [Key]
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Identity { get; set; }
        public string AvatarUrl { get; set; }
        public string Github { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public IEnumerable<string> Claims { get { return UserClaims.Select(s => s.Claim.Name); } set { } } // User Admin levels claims - https://github.com/NancyFx/Nancy/blob/master/src/Nancy.Demo.Authentication/AuthenticationBootstrapper.cs

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<UserClaim> UserClaims { get; set; }
    }
}