using System;
using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }

        public DateTime Time { get; set; }
        public string Text { get; set; }

        public virtual Idea Idea { get; set; }

        public virtual User User { get; set; }
    }
}