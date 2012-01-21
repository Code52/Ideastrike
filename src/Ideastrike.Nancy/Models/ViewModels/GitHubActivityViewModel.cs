using Ideastrike.Nancy.Helpers;

namespace Ideastrike.Nancy.Models.ViewModels
{
    // TODO: get user context working properly

    public class GitHubActivityViewModel
    {
        public GitHubActivityViewModel(GitHubActivity github)
        {
            Text = github.Message;
            AuthorName = github.AuthorName;
            AuthorUrl = github.AuthorUrl;
            Sha = github.Sha;
            ShaShortened = github.Sha.Substring(0, 8);
            GravatarUrl = github.GravatarUrl;
            FriendlyTime = FriendlyTimeHelper.Parse(github.Time).ToHtmlString();
        }

        public string Text { get; set; }
        public string AuthorName { get; set; }
        public string AuthorUrl { get; set; }
        public string Sha { get; set; }
        public string ShaShortened { get; set; }
        public string CommitUrl { get; set; }
        public string GravatarUrl { get; set; }
        public string FriendlyTime { get; set; }
    }
}