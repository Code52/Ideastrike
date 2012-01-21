using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class Idea
    {
        public Idea()
        {
            Activities = new Collection<Activity>();
            Votes = new Collection<Vote>();
			Features = new Collection<Feature>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public User Author { get; set; }
		public ICollection<Vote> Votes { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Feature> Features { get; set; }
    }
}