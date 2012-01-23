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

        public when_adding_a_new_comment()
        {
            testResponse = browser.Post("/comment/0/add", with => {
                with.FormValue("userId", "1");
                with.FormValue("comment", "words");
                with.LoggedInUser(CreateMockUser("shiftkey"));
            });
        }

        [Fact]
        public void it_should_add_the_comment()
        {
            mockActivityRepo.Verify(B => B.Add(0, It.IsAny<Comment>()));
        }
    }
}

