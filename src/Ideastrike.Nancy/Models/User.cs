using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nancy;
using Nancy.Security;

namespace Ideastrike.Nancy.Models
{
    public class User: IUserIdentity
    {
        [Key]
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Identity { get; set; }
        public string AvatarUrl { get; set; }
        public string Github { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public IEnumerable<string> Claims { get; set; } // User Admin levels claims - https://github.com/NancyFx/Nancy/blob/master/src/Nancy.Demo.Authentication/AuthenticationBootstrapper.cs

        public virtual ICollection<Vote> Votes { get; set; }
    }
}