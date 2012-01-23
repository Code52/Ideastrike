using System;
using System.Collections.Generic;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Moq;
using Nancy;
using Nancy.Security;
using Nancy.Testing;
using System.Linq;
using System.Linq.Expressions;

namespace IdeaStrike.Tests
{
    public class IdeaStrikeSpecBase
    {
        protected Mock<IActivityRepository> mockActivityRepo;
        protected Mock<IFeatureRepository> mockFeatureRepo;
        protected Mock<IIdeaRepository> mockIdeasRepo;
        protected Mock<IdeastrikeContext> mockIdeaStrikeContext;
        protected Mock<ISettingsRepository> mockSettingsRepo;
        protected Mock<IUserRepository> mockUsersRepo;
        protected Mock<IImageRepository> mockImageRepo;

        protected BrowserResponse testResponse;
        protected Browser browser;
        protected IdeaStrikeTestBootStrapper context;

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

        protected User CreateMockUser(string name) {
            var user = new User {
                Id= Guid.NewGuid(),
                UserName= name
            };
            mockUsersRepo.Setup(d => d.Get(user.Id)).Returns(user);
            mockUsersRepo.Setup(d => d.GetUserFromIdentifier(user.Id)).Returns(user);
            mockUsersRepo.Setup(d => d.FindBy(It.IsAny<Expression<Func<User,bool>>>())).Returns(new [] { user }.AsQueryable());
            return user;
        }

        protected Idea CreateMockIdea(Idea idea) {
            mockIdeasRepo.Setup(d => d.Get(idea.Id)).Returns(idea);
            return idea;
        }
    }
}

