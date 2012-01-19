using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class Vote
    {
        public int Value { get; set; }

        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int IdeaId { get; set; }
    }
}