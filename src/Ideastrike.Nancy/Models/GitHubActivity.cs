namespace Ideastrike.Nancy.Models
{
    public class GitHubActivity : Activity
    {
        public User Author { get; set; }
        public string Message { get; set; }
        public string AuthorUrl { get; set; }
        public string Sha { get; set; }
        public string CommitUrl { get; set; }
        public string GravatarUrl { get; set; }
        public string AuthorName { get; set; }
    }
}