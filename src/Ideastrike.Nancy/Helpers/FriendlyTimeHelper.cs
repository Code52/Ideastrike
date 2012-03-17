using System;
using Nancy.ViewEngines.Razor;

namespace Ideastrike.Nancy.Helpers
{
    public static class FriendlyTimeHelper
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        public static IHtmlString Parse(DateTime dt)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);
            if (delta < 0)
            {
                return HtmlString("not yet");
            }
            if (delta < 1 * MINUTE)
            {
                return HtmlString(ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago");
            }
            if (delta < 2 * MINUTE)
            {
                return HtmlString("a minute ago");
            }
            if (delta < 45 * MINUTE)
            {
                return HtmlString(ts.Minutes + " minutes ago");
            }
            if (delta < 90 * MINUTE)
            {
                return HtmlString("an hour ago");
            }
            if (delta < 24 * HOUR)
            {
                return HtmlString(ts.Hours + " hours ago");
            }
            if (delta < 48 * HOUR)
            {
                return HtmlString("yesterday");
            }
            if (delta < 30 * DAY)
            {
                return HtmlString(ts.Days + " days ago");
            }
            if (delta < 12 * MONTH)
            {
                var months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return HtmlString(months <= 1 ? "one month ago" : months + " months ago");
            }
            
            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return HtmlString(years <= 1 ? "one year ago" : years + " years ago");
        }

        private static IHtmlString HtmlString(string value)
        {
            return value.ToHtmlString();
        }
    }
}