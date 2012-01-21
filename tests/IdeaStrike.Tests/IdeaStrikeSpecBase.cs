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
        public Mock<IActivityRepository> mockActivityRepo;
        public Mock<IFeatureRepository> mockFeatureRepo;
        public Mock<IIdeaRepository> mockIdeasRepo;
        public Mock<IdeastrikeContext> mockIdeaStrikeContext;
        public Mock<ISettingsRepository> mockSettingsRepo;
        
        protected Response testResponse;
        protected INancyEngine engine;

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
            mockActivityRepo = new Mock<IActivityRepository>();
            mockFeatureRepo = new Mock<IFeatureRepository>();
            mockIdeasRepo = new Mock<IIdeaRepository>();
            mockSettingsRepo = new Mock<ISettingsRepository>();
            mockIdeaStrikeContext = new Mock<IdeastrikeContext>();
        }

        private ContainerBuilder CreateContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<ISettingsRepository>(mockSettingsRepo.Object);
            builder.RegisterInstance<IIdeaRepository>(mockIdeasRepo.Object);
            builder.RegisterInstance<IActivityRepository>(mockActivityRepo.Object);
            builder.RegisterInstance<IFeatureRepository>(mockFeatureRepo.Object);
            builder.RegisterInstance<IdeastrikeContext>(mockIdeaStrikeContext.Object);
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

