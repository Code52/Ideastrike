using Nancy;
using Xunit;

namespace IdeaStrike.Tests.AdminModuleTests
{
    public class when_an_unauthenticated_user_views_the_admin_page : IdeaStrikeSpecBase
    {
        public when_an_unauthenticated_user_views_the_admin_page()
        {
            var testRequest = GetTestRequest("/admin");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_redirect_to_the_login_page()
        {
            Assert.Equal(HttpStatusCode.SeeOther, testResponse.StatusCode);
            Assert.Equal("/login", testResponse.Headers["Location"]);
        }
    }
}
