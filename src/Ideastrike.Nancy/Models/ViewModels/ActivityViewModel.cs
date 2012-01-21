using System.Web;
using Ideastrike.Nancy.Helpers;

namespace Ideastrike.Nancy.Models.ViewModels
{
    public class ActivityViewModel
    {
        public ActivityViewModel(Activity activity)
        {
            FriendlyTime = FriendlyTimeHelper.Parse(activity.Time);

            var comment = activity as Comment;
            if (comment != null)
            {
                Text = MarkdownHelper.Markdown(comment.Text);
                // TODO: not hard code these
                Author = "shiftkey";
                GravatarUrl = "me@brendanforster.com".ToGravatarUrl(40);
            }
        }

        public IHtmlString FriendlyTime { get; private set; }

        public IHtmlString Text { get; private set; }
        public string Author { get; private set; }
        public string GravatarUrl { get; private set; }
    }
}