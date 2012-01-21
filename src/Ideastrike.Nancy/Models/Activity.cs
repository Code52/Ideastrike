using System;
using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        
        public virtual Idea Idea { get; set; }

        public virtual User User { get; set; }

        public DateTime Time { get; set; }
    }
}