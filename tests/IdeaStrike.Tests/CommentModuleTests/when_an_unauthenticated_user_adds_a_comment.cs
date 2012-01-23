using Nancy;
using Nancy.Testing;
using Xunit;

namespace IdeaStrike.Tests.CommentModuleTests
{
    public class when_an_unauthenticated_user_adds_a_comment : IdeaStrikeSpecBase
    {
        public when_an_unauthenticated_user_adds_a_comment()
        {
            testResponse = browser.Post("/comment/0/add");
        }

        [Fact]
        public void it_should_redirect_to_the_login_page()
        {
            testResponse.ShouldHaveRedirectedTo("/login");
        }
    }
}