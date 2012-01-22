using System;
using System.Collections.Generic;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Moq;
using Nancy;
using Nancy.Security;
using Nancy.Testing;

namespace IdeaStrike.Tests
{
    public class IdeaStrikeSpecBase
    {
        public Mock<IActivityRepository> mockActivityRepo;
        public Mock<IFeatureRepository> mockFeatureRepo;
        public Mock<IIdeaRepository> mockIdeasRepo;
        public Mock<IdeastrikeContext> mockIdeaStrikeContext;
        public Mock<ISettingsRepository> mockSettingsRepo;
        public Mock<IUserRepository> mockUsersRepo;
        public Mock<IImageRepository> mockImageRepo;

        protected Response testResponse;
        protected INancyEngine engine;
        protected Browser browser;
        public IdeaStrikeTestBootStrapper context;

        public IdeaStrikeSpecBase()
        {
            CreateMocks();

            var mocks = new Dictionary<Type, object>
                            {
                                { typeof (IActivityRepository), mockActivityRepo.Object },
                                { typeof (IFeatureRepository), mockFeatureRepo.Object },
                                { typeof (IIdeaRepository),mockIdeasRepo.Object },
                                { typeof (ISettingsRepository),mockSettingsRepo.Object },
                                { typeof (IUserRepository),mockUsersRepo.Object },
                                { typeof (IImageRepository),mockImageRepo.Object },
                                { typeof (IdeastrikeContext),mockIdeaStrikeContext.Object }
                            };

            context = new IdeaStrikeTestBootStrapper(mocks);
            context.Initialise();

            engine = context.GetEngine();
            browser = new Browser(context);
        }

        private void CreateMocks()
        {
            mockActivityRepo = new Mock<IActivityRepository>();
            mockFeatureRepo = new Mock<IFeatureRepository>();
            mockIdeasRepo = new Mock<IIdeaRepository>();
            mockSettingsRepo = new Mock<ISettingsRepository>();
            mockUsersRepo = new Mock<IUserRepository>();
            mockImageRepo = new Mock<IImageRepository>();
            mockIdeaStrikeContext = new Mock<IdeastrikeContext>();
        }

        public static IUserIdentity CreateMockUser(string name)
        {
            var user = new Mock<IUserIdentity>();
            user.Setup(i => i.UserName).Returns(name);
            return user.Object;
        }

        public static Response AuthenticateUser(NancyContext arg, string username)
        {
            arg.CurrentUser = CreateMockUser(username);
            return null;
        }


        public virtual void BeforeRequest()
        {

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

        protected void RunFirst(Func<NancyContext, Response> authenticateUser)
        {
            context.BeforeRequest.AddItemToStartOfPipeline(authenticateUser);
        }
    }
}

