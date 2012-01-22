using Nancy;
using Xunit;

namespace IdeaStrike.Tests.AdminModuleTests
{
    public class when_an_authenticated_user_views_the_admin_page : IdeaStrikeSpecBase
    {
        public when_an_authenticated_user_views_the_admin_page()
        {
            var testRequest = GetTestRequest("/admin");
            RunFirst(r => AuthenticateUser(r, "shiftkey"));
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_set_the_status_code_to_unauthorized_for_the_admin_page()
        {
            Assert.Equal(HttpStatusCode.OK, testResponse.StatusCode);
        }
    }
}