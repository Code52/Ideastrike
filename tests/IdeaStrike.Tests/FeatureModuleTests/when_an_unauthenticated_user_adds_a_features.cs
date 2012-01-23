using Xunit;
using Nancy;
using Nancy.Testing;

namespace IdeaStrike.Tests.FeatureModuleTests
{
    public class when_an_unauthenticated_user_adds_a_features : IdeaStrikeSpecBase
    {
        public when_an_unauthenticated_user_adds_a_features()
        {
            testResponse = browser.Post("/idea/0/feature");
        }

        [Fact]
        public void it_should_redirect_to_the_login_page() {
            testResponse.ShouldHaveRedirectedTo("/login");
        }
    }
}
