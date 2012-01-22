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
    public class when_voting_for_an_item : IdeaStrikeSpecBase
    {
        private Guid _ideaGuid  = Guid.NewGuid();

        public when_voting_for_an_item()
        {
            var users = new List<User> { new User { UserName = "shiftkey", Id= _ideaGuid } };
            mockUsersRepo.Setup(u => u.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                         .Returns(users.AsQueryable());

            var testRequest = PostTestRequest("/idea/1/vote");
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
        public void it_should_register_the_vote()
        {
            mockIdeasRepo.Verify(b => b.Vote(1, _ideaGuid, 1));
        }
    }
}

