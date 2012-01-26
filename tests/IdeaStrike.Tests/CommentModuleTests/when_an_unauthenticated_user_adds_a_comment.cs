using Nancy;
using Nancy.Testing;
using Xunit;
using Ideastrike.Nancy.Models;
using Moq;
using Ideastrike.Nancy.Modules;

namespace IdeaStrike.Tests.CommentModuleTests
{
    public class when_an_unauthenticated_user_adds_a_comment : IdeaStrikeSpecBase<CommentModule>
    {
        private Mock<IUserRepository> _Users = new Mock<IUserRepository>();

        public when_an_unauthenticated_user_adds_a_comment()
        {
            Configure(_Users.Object);
            EnableFormsAuth(_Users);
            Post("/comment/0/add");
        }

        [Fact]
        public void it_should_redirect_to_the_login_page()
        {
            Response.ShouldHaveRedirectedTo("/login");
        }
    }
}