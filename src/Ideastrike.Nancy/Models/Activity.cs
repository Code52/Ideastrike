using System;
using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class Activity
    {
        [Key]
        public virtual int Id { get; set; }

        public Idea Idea { get; set; }

        public DateTime Time { get; set; }
    }
}