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
using Autofac;
using Ideastrike.Nancy;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Moq;
namespace IdeaStrike.Tests
{
    public class IdeaStrikeSpecBase
    {
        public IActivityRepository mockActivityRepo = null;
        public IFeatureRepository mockFeatureRepo = null;
        public IIdeaRepository mockIdeasRepo = null;
        public IdeastrikeContext mockIdeaStrikeContext = null;
        public ISettingsRepository mockSettingsRepo = null;

        protected INancyEngine engine;
        protected Response testResponse;
        public IdeaStrikeSpecBase()
        {
            CreateMocks();
            ContainerBuilder builder = CreateContainerBuilder();

            var ideaStrikeTestBootstrapper = new IdeaStrikeTestBootStrapper(builder);
            ideaStrikeTestBootstrapper.Initialise();
            engine = ideaStrikeTestBootstrapper.GetEngine();
        }

        private void CreateMocks()
        {
            mockActivityRepo = new Mock<IActivityRepository>().Object;
            mockFeatureRepo = new Mock<IFeatureRepository>().Object;
            mockIdeasRepo = new Mock<IIdeaRepository>().Object;
            mockSettingsRepo = new Mock<ISettingsRepository>().Object;
            mockIdeaStrikeContext = new Mock<IdeastrikeContext>().Object;
        }

        private ContainerBuilder CreateContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<ISettingsRepository>(mockSettingsRepo);
            builder.RegisterInstance<IIdeaRepository>(mockIdeasRepo);
            builder.RegisterInstance<IActivityRepository>(mockActivityRepo);
            builder.RegisterInstance<IFeatureRepository>(mockFeatureRepo);
            builder.RegisterInstance<IdeastrikeContext>(mockIdeaStrikeContext);
            return builder;
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

