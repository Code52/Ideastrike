using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class VotesToUser
    {
        
        public int Value { get; set; }

        [Key]
        public User User { get; set; }

        [Key]
        public Idea Idea { get; set; }
    }
}