using Ideastrike.Nancy.Models;
using Moq;
using Nancy;
using Xunit;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_an_unauthorized_user_creates_a_new_idea : IdeaStrikeSpecBase
    {
        public when_an_unauthorized_user_creates_a_new_idea()
        {
            var testRequest = PostTestRequest("/idea/new");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_redirect_to_the_login_page()
        {
            Assert.Equal(HttpStatusCode.SeeOther, testResponse.StatusCode);
            Assert.Equal("/login", testResponse.Headers["Location"]);
        }

        [Fact]
        public void it_should_not_add_a_new_idea_to_the_repository()
        {
            mockIdeasRepo.Verify(B => B.Add(It.IsAny<Idea>()), Times.Never());
        }
    }
}