using Nancy;
using Nancy.Testing;
using Xunit;
using Ideastrike.Nancy.Modules;
using Moq;
using Ideastrike.Nancy.Models;

namespace IdeaStrike.Tests.AdminModuleTests
{
    public class when_an_unauthenticated_user_views_the_admin_page : IdeaStrikeSpecBase<AdminModule>
    {
        public when_an_unauthenticated_user_views_the_admin_page()
        {
            Configure();
            EnableFormsAuth(new Mock<IUserRepository>());
            Get("/admin");
        }

        [Fact]
        public void it_should_redirect_to_the_login_page()
        {
            Response.ShouldHaveRedirectedTo("/login");
        }
    }
}
