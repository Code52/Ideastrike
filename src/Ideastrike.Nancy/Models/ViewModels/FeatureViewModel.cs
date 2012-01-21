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

            // TODO: not hard code these
            Author = "shiftkey";
            GravatarUrl = "me@brendanforster.com".ToGravatarUrl(40);
        }

        public IHtmlString FriendlyTime { get; private set; }

        public IHtmlString Text { get; private set; }
        public string Author { get; private set; }
        public string GravatarUrl { get; private set; }
    }
}