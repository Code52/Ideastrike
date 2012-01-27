using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using Nancy;

namespace Ideastrike.Nancy.Helpers
{
    public class FeedHelpers
    {
        public class FeedResponse<TModel> : Response
        {
            public FeedResponse(TModel model)
            {
                this.Contents = GetXmlContents(model);
                this.ContentType = "application/atom+xml";
                this.StatusCode = HttpStatusCode.OK;
            }

            private static Action<Stream> GetXmlContents(TModel model)
            {
                var m = model as Atom10FeedFormatter;

                return stream =>
                {
                    using (XmlWriter writer = XmlWriter.Create(stream))
                    {
                        m.WriteTo(writer);
                    }
                };
            }
        }
    }

    public static class FormatterExtentions
    {
        public static Response AsFeed<TModel>(this IResponseFormatter formatter, TModel model)
        {
            return new FeedHelpers.FeedResponse<TModel>(model);
        }
    }

}