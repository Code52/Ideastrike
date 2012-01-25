using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ideastrike.Nancy.Models
{
    public class Image : IEqualityComparer<Image>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] ImageBits { get; set; }

        public int? IdeaId { get; set; }

        public int CompareTo(Image other)
        {
            return this.Id.CompareTo(other.Id);
        }

        public bool Equals(Image x, Image y)
        {
            if(x == null && y == null)
            {
                return true;
            }

            if(x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id;

        }

        public int GetHashCode(Image obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}