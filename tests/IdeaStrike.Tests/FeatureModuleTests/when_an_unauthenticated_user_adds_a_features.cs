using Xunit;
using Nancy;
using Nancy.Testing;
using Ideastrike.Nancy.Modules;
using Ideastrike.Nancy.Models;
using Moq;

namespace IdeaStrike.Tests.FeatureModuleTests
{
    public class when_an_unauthenticated_user_adds_a_features : IdeaStrikeSpecBase<FeatureModule>
    {
        public Mock<IUserRepository> _Users = new Mock<IUserRepository>();

        public when_an_unauthenticated_user_adds_a_features()
        {
            Configure(_Users.Object);
            EnableFormsAuth(_Users);
            Post("/idea/0/feature");
        }

        [Fact]
        public void it_should_redirect_to_the_login_page() {
            Response.ShouldHaveRedirectedTo("/login");
        }
    }
}
