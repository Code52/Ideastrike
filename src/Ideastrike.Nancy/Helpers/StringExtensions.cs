using Nancy.ViewEngines.Razor;

namespace Ideastrike.Nancy.Helpers
{
    public static class StringExtensions
    {
        public static IHtmlString ToHtmlString(this string s)
        {
            return new NonEncodedHtmlString(s);
        }
    }
}