using System.Web;
using Ideastrike.Nancy.Helpers;

namespace Ideastrike.Nancy.Models.ViewModels
{
    public class FeatureViewModel
    {
        public FeatureViewModel(Feature feature)
        {
            Text = MarkdownHelper.Markdown(feature.Text);
            FriendlyTime = FriendlyTimeHelper.Parse(feature.Time);
            Author = feature.User.UserName;
            GravatarUrl = (string.IsNullOrEmpty(feature.User.AvatarUrl)) ? feature.User.Email.ToGravatarUrl(40) : feature.User.AvatarUrl;
        }

        public IHtmlString FriendlyTime { get; private set; }

        public IHtmlString Text { get; private set; }
        public string Author { get; private set; }
        public string GravatarUrl { get; private set; }
    }
}