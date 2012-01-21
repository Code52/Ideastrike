using Ideastrike.Nancy.Helpers;

namespace Ideastrike.Nancy.Models.ViewModels
{
    // TODO: user values should not be hard coded
    
    public class CommentViewModel
    {
        public CommentViewModel(Comment comment)
        {
            FriendlyTime = FriendlyTimeHelper.Parse(comment.Time).ToHtmlString();
            Text = MarkdownHelper.Markdown(comment.Text).ToHtmlString();

            Author = "shiftkey";
            GravatarUrl = "me@brendanforster.com".ToGravatarUrl(40);
        }

        public string FriendlyTime { get; private set; }

        public string Text { get; private set; }
        public string Author { get; private set; }
        public string GravatarUrl { get; private set; }
    }
}