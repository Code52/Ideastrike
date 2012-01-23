using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ideastrike.Nancy.Models;
using Moq;
using Xunit;

namespace IdeaStrike.Tests.CommentModuleTests
{
    // TODO: test that an unauthenticated user doesn't get access to this resource

    public class when_adding_a_new_comment : IdeaStrikeSpecBase
    {
        private static string _userName = "shiftkey";

        public when_adding_a_new_comment()
        {
            var users = new[] { new User { UserName = _userName } };
            mockUsersRepo.Setup(u => u.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                         .Returns(users.AsQueryable());

            RunFirst(r => AuthenticateUser(r, _userName));

            var testRequest = PostTestRequest("/comment/0/add");
            testRequest.Form.userId = 1;
            testRequest.Form.comment = "words";
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_add_the_comment()
        {
            mockActivityRepo.Verify(B => B.Add(0, It.IsAny<Comment>()));
        }
    }
}

