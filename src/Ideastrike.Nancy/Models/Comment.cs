using System.ComponentModel.DataAnnotations;
namespace Ideastrike.Nancy.Models
{
    public class Comment : Activity
    {
        [Key]
        public int Id { get; set; }
        public int IdeaId { get; set; }
        
    }
}