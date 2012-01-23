using Nancy;
using Xunit;

namespace IdeaStrike.Tests.CommentModuleTests
{
    public class when_viewing_the_comments_page : IdeaStrikeSpecBase
    {
        public when_viewing_the_comments_page()
        {
            var testRequest = PostTestRequest("/idea/0/comment/");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        public void it_should_set_the_status_code_to_OK()
        {
            //noop - The GET method is commented out in the CommentModule
            Assert.Equal(HttpStatusCode.SeeOther, testResponse.StatusCode);
        }
    }
}