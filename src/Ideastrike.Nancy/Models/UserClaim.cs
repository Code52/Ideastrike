using System;
using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class UserClaim
    {
        public virtual User User { get; set; }
        public virtual Claim Claim { get; set; }

        [Key, ForeignKey("User"), Column(Order = 0)]
        public Guid UserId { get; set; }

        [Key, ForeignKey("Claim"), Column(Order = 1)]
        public int ClaimId { get; set; }
    }
}