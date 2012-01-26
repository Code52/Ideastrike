using Nancy;
using Xunit;
using Nancy.Testing;
using Ideastrike.Nancy.Modules;

namespace IdeaStrike.Tests.HomeModuleTests
{
    public class when_viewing_the_home_page : IdeaStrikeSpecBase<HomeModule>
    {
        public when_viewing_the_home_page()
        {
            Configure();
            Get("/");
        }

        [Fact]
        public void it_should_set_the_status_code_to_ok_for_the_home_page()
        {
            Assert.Equal(HttpStatusCode.OK, Response.StatusCode);
        }
    }
}
