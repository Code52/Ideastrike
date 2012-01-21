using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public string FullName { get; set; }
    }
}