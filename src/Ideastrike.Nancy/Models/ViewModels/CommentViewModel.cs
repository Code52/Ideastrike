using Ideastrike.Nancy.Helpers;

namespace Ideastrike.Nancy.Models.ViewModels
{
    public class CommentViewModel
    {
        public CommentViewModel(Comment comment)
        {
            FriendlyTime = FriendlyTimeHelper.Parse(comment.Time).ToHtmlString(); // this is encoding when it shouldn't be
            Text = comment.Text;

            Author = comment.User.UserName;
            GravatarUrl = (string.IsNullOrEmpty(comment.User.AvatarUrl)) ? comment.User.Email.ToGravatarUrl(40) : comment.User.AvatarUrl;
        }

        public string FriendlyTime { get; private set; }

        public string Text { get; private set; }
        public string Author { get; private set; }
        public string GravatarUrl { get; private set; }
    }
}