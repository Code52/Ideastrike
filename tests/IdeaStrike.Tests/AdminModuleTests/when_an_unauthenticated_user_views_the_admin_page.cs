using Nancy;
using Nancy.Testing;
using Xunit;

namespace IdeaStrike.Tests.AdminModuleTests
{
    public class when_an_unauthenticated_user_views_the_admin_page : IdeaStrikeSpecBase
    {
        public when_an_unauthenticated_user_views_the_admin_page()
        {
            testResponse = browser.Get("/admin");
        }

        [Fact]
        public void it_should_redirect_to_the_login_page()
        {
            testResponse.ShouldHaveRedirectedTo("/login");
        }
    }
}
