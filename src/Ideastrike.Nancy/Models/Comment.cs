using System.ComponentModel.DataAnnotations;
namespace Ideastrike.Nancy.Models
{
    public class Comment : Activity
    {
        [Key]
        public int Id { get; set; }

        public int IdeaId { get; set; }

        public string Text { get; set; }

        public int UserId { get; set; }
    }
}