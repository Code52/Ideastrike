using System;
using System.Web;
using System.Web.Mvc;

namespace Ideastrike.Nancy.Helpers
{
    public static partial class StatusColorHelper
    {
        public static IHtmlString ColorClass(string Status)
        {
            switch (Status)
            {
                case "New":
                    return new MvcHtmlString("warning");
                case "Active":
                    return new MvcHtmlString("success");
                case "Declined":
                    return new MvcHtmlString("important");
                case "Completed":
                    return new MvcHtmlString("notice");
                default:
                    return new MvcHtmlString(String.Empty);
            }
        }
    }
}