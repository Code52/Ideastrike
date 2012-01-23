using Nancy;
using Xunit;
using Nancy.Testing;

namespace IdeaStrike.Tests.HomeModuleTests
{
    public class when_viewing_the_home_page : IdeaStrikeSpecBase
    {
        public when_viewing_the_home_page()
        {
            testResponse = browser.Get("/");
        }

        [Fact]
        public void it_should_set_the_status_code_to_ok_for_the_home_page()
        {
            Assert.Equal(HttpStatusCode.OK, testResponse.StatusCode);
        }
    }
}
