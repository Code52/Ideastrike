using Ideastrike.Nancy.Modules;
using Nancy;
using Nancy.Responses;
using Xunit;

namespace IdeaStrike.Tests.AdminModuleTests
{
    public class when_an_authenticated_user_views_the_admin_page : IdeaStrikeSpecBase<AdminModule>
    {
        public when_an_authenticated_user_views_the_admin_page()
        {
            SetView("Admin/Index", new HtmlResponse());
            EnableFormsAuth();
            Get("/admin", with => with.LoggedInUser(CreateMockUser("shiftkey")));
        }

        [Fact]
        public void it_should_set_the_status_code_to_ok()
        {
            Assert.Equal(HttpStatusCode.OK, Response.StatusCode);
        }
    }
}