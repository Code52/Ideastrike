using Nancy;
using Xunit;

namespace IdeaStrike.Tests.AdminModuleTests
{
    public class when_an_authenticated_user_views_the_admin_page : IdeaStrikeSpecBase
    {
        public when_an_authenticated_user_views_the_admin_page()
        {
            testResponse = browser.Get("/admin", with => {
                with.LoggedInUser(CreateMockUser("shiftkey"));
            });
        }

        [Fact]
        public void it_should_set_the_status_code_to_unauthorized_for_the_admin_page()
        {
            Assert.Equal(HttpStatusCode.OK, testResponse.StatusCode);
        }
    }
}