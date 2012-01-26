using System;
using System.Linq;
using System.Web.Management;
using Nancy;
using Nancy.ErrorHandling;

namespace Ideastrike.Nancy.Helpers
{
    public class ElmahErrorHandler : IErrorHandler
    {
        private readonly HttpStatusCode[] _supportedStatusCodes = new[]
    {
        HttpStatusCode.InternalServerError,
        HttpStatusCode.BadRequest,
        HttpStatusCode.NotFound
    };

        public bool HandlesStatusCode(HttpStatusCode statusCode)
        {
            return _supportedStatusCodes.Any(s => s == statusCode);
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var message = string.Format("Error occurred for requested URL: {0}.", getUrl(context));
            new LogEvent(message).Raise();
        }

        private static string getUrl(NancyContext context)
        {
            return string.Concat(context.Request.Path, context.Request.Url.Query);
        }

        public class LogEvent : WebRequestErrorEvent
        {
            public LogEvent(string message)
                : base(null, null, 100001, new Exception(message))
            {
            }
        }
    }
}