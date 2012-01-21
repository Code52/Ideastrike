using System;
using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class Feature
    {
        [Key]
        public virtual int Id { get; set; }

        public DateTime Time { get; set; }
        public Idea Idea { get; set; }
        public string Text { get; set; }
    }
}