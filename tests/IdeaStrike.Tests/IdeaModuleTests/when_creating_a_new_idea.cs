using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Xunit;
using Moq;
using Ideastrike.Nancy.Models;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_creating_a_new_idea : IdeaStrikeSpecBase
    {
        public when_creating_a_new_idea()
        {
            var testRequest = PostTestRequest("/idea/");
            testResponse = engine.HandleRequest(testRequest).Response;
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

