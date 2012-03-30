using Nancy.ViewEngines.Razor;

namespace Ideastrike.Nancy.Helpers
{
    public static class StatusColorHelper
    {
        public static IHtmlString ColorClass(string status)
        {
            switch (status)
            {
                case "New":
                    return "warning".ToHtmlString();
                case "Active":
                    return "success".ToHtmlString();
                case "Declined":
                    return "important".ToHtmlString();
                case "Completed":
                    return "notice".ToHtmlString();
                default:
                    return string.Empty.ToHtmlString();
            }
        }
    }
}