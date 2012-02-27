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
                return new NonEncodedHtmlString("not yet");
            }
            if (delta < 1 * MINUTE)
            {
                return new NonEncodedHtmlString(ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago");
            }
            if (delta < 2 * MINUTE)
            {
                return new NonEncodedHtmlString("a minute ago");
            }
            if (delta < 45 * MINUTE)
            {
                return new NonEncodedHtmlString(ts.Minutes + " minutes ago");
            }
            if (delta < 90 * MINUTE)
            {
                return new NonEncodedHtmlString("an hour ago");
            }
            if (delta < 24 * HOUR)
            {
                return new NonEncodedHtmlString(ts.Hours + " hours ago");
            }
            if (delta < 48 * HOUR)
            {
                return new NonEncodedHtmlString("yesterday");
            }
            if (delta < 30 * DAY)
            {
                return new NonEncodedHtmlString(ts.Days + " days ago");
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return new NonEncodedHtmlString(months <= 1 ? "one month ago" : months + " months ago");
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return new NonEncodedHtmlString(years <= 1 ? "one year ago" : years + " years ago");
            }

        }
    }
}