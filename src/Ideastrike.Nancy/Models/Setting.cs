using System.ComponentModel.DataAnnotations;

namespace Ideastrike.Nancy.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Name { get; set; }
        public string WelcomeMessage { get; set; }
        public string HomePage { get; set; }
        public string GAnalyticsKey { get; set; }
    }
}