using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.Bootstrapper;
using TinyIoC;
using Ideastrike.Nancy;
using Autofac;
using Ideastrike.Nancy.Models.Repositories;
using Ideastrike.Nancy.Models;

namespace IdeaStrike.Tests
{
    public class IdeaStrikeSpecBase
    {
        protected INancyEngine engine;
        protected Response testResponse;
        public IdeaStrikeSpecBase()
        {
            var ideaStrikeTestBootstrapper = new IdeaStrikeTestBootStrapper();
            ideaStrikeTestBootstrapper.Initialise();
            engine = ideaStrikeTestBootstrapper.GetEngine();
        }

        private static Request CreateTestRequest(string httpMethod, string route)
        {
            return new Request(httpMethod, route, "http");
        }

        protected static Request GetTestRequest(string route)
        {
            return CreateTestRequest("Get", route);
        }

        public static Request PostTestRequest(string route)
        {
            return CreateTestRequest("POST", route);
        }
    }
}

