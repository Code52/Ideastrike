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
        public string Status { get; set; }
        public DateTime Time { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Feature> Features { get; set; }
        public virtual User Author { get; set; }

        [NotMapped]
        public bool UserHasVoted { get; set; }
		
        public virtual ICollection<Image> Images { get; set; } 
    }
}