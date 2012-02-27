using Nancy.ViewEngines.Razor;

namespace Ideastrike.Nancy.Helpers
{
    public static partial class StatusColorHelper
    {
        public static IHtmlString ColorClass(string Status)
        {
            switch (Status)
            {
                case "New":
                    return new NonEncodedHtmlString("warning");
                case "Active":
                    return new NonEncodedHtmlString("success");
                case "Declined":
                    return new NonEncodedHtmlString("important");
                case "Completed":
                    return new NonEncodedHtmlString("notice");
                default:
                    return new NonEncodedHtmlString(string.Empty);
            }
        }
    }
}