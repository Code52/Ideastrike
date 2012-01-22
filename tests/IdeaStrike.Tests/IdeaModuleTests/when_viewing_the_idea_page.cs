using Nancy;
using Xunit;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_viewing_the_idea_page : IdeaStrikeSpecBase
    {
        public when_viewing_the_idea_page()
        {
            var testRequest = GetTestRequest("/idea/0/");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_set_the_status_code_to_ok_for_the_idea_page()
        {
            Assert.Equal(HttpStatusCode.OK, testResponse.StatusCode);
        }
    }
}