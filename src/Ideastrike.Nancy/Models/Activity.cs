using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class Activity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}