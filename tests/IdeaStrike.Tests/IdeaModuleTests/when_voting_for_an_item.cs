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
        Guid _ideaGuid = Guid.NewGuid();
        const string UserName = "shiftkey";

        public when_voting_for_an_item()
        {
            var users = new[] { new User { UserName = UserName, Id = _ideaGuid } };
            mockUsersRepo.Setup(u => u.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                         .Returns(users.AsQueryable());

            RunFirst(r => AuthenticateUser(r, UserName));

            var testRequest = PostTestRequest("/idea/1/vote");

            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_register_the_vote()
        {
            mockIdeasRepo.Verify(b => b.Vote(1, _ideaGuid, 1));
        }
    }
}

