using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nancy;
using Nancy.Security;
using Xunit;
using Moq;
using Ideastrike.Nancy.Models;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_creating_a_new_idea : IdeaStrikeSpecBase
    {
        public when_creating_a_new_idea()
        {
            var users = new List<User> {new User {UserName = "shiftkey"}};
            mockUsersRepo.Setup(u => u.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                         .Returns(users.AsQueryable());

            var testRequest = PostTestRequest("/idea/new");
            RunBefore(AuthenticateUser);
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        private static Response AuthenticateUser(NancyContext arg)
        {
            var user = new Mock<IUserIdentity>();
            user.Setup(i => i.UserName).Returns("shiftkey");
            arg.CurrentUser = user.Object;
            return null;
        }

        [Fact]
        public void it_should_redirect_to_the_new_idea()
        {
            Assert.Equal(HttpStatusCode.SeeOther, testResponse.StatusCode);
        }

        [Fact]
        public void it_should_add_a_new_idea_to_the_repository()
        {
            mockIdeasRepo.Verify(B => B.Add(It.IsAny<Idea>()));
        }
    }
}

