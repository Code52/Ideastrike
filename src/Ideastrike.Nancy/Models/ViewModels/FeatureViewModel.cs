using System;
using System.Web;
using Ideastrike.Nancy.Helpers;

namespace Ideastrike.Nancy.Models.ViewModels
{
    public class FeatureViewModel
    {
        public FeatureViewModel(Feature feature)
        {
            Text = MarkdownHelper.Markdown(feature.Text);
            Time = feature.Time;
        }

        public DateTime Time { get; private set; }

        public IHtmlString Text { get; private set; }
    }
}