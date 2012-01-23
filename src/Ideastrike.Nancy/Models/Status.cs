using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}