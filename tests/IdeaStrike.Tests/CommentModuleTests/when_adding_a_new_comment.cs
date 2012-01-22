using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ideastrike.Nancy.Models;
using Moq;
using Nancy;
using Nancy.Security;
using Xunit;

namespace IdeaStrike.Tests.CommentModuleTests
{
    // TODO: test that an unauthenticated user doesn't get access to this resource

    public class when_adding_a_new_comment : IdeaStrikeSpecBase
    {
        public when_adding_a_new_comment()
        {
            var users = new List<User> { new User { UserName = "shiftkey" } };
            mockUsersRepo.Setup(u => u.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                         .Returns(users.AsQueryable());

            RunBefore(AuthenticateUser);

            var testRequest = PostTestRequest("/comment/0/add");
            testRequest.Form.userId = 1;
            testRequest.Form.comment = "words";
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
        public void it_should_add_the_comment()
        {
            mockActivityRepo.Verify(B => B.Add(0, It.IsAny<Comment>()));
        }
    }
}

