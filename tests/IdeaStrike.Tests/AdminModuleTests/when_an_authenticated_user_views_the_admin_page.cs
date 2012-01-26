using Nancy;
using Xunit;
using Nancy.Testing;
using Ideastrike.Nancy.Modules;
using Ideastrike.Nancy.Models;
using Moq;

namespace IdeaStrike.Tests.AdminModuleTests
{
    public class when_an_authenticated_user_views_the_admin_page : IdeaStrikeSpecBase<AdminModule>
    {
        private Mock<IUserRepository> _Users = new Mock<IUserRepository>();

        public when_an_authenticated_user_views_the_admin_page()
        {
            Configure(_Users.Object);
            EnableFormsAuth(_Users);
            
            Get("/admin", with => {
                with.LoggedInUser(CreateMockUser(_Users, "shiftkey"));
            });
        }

        [Fact]
        public void it_should_set_the_status_code_to_ok()
        {
            Assert.Equal(HttpStatusCode.OK, Response.StatusCode);
        }
    }
}