using System;
using System.Collections.Generic;
using System.Linq;
using Ideastrike.Nancy.Models;
using Moq;
using Xunit;

namespace IdeaStrike.Tests.CommentModuleTests
{
    public class when_adding_a_new_comment : IdeaStrikeSpecBase
    {
        public when_adding_a_new_comment()
        {
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

