using Ideastrike.Nancy.Models;
using Moq;
using Nancy;
using Xunit;
using Nancy.Testing;
using Ideastrike.Nancy.Modules;
using Ideastrike.Nancy.Models.Repositories;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_an_unauthorized_user_creates_a_new_idea : IdeaStrikeSpecBase<IdeaSecuredModule>
    {
        private Mock<IUserRepository> _Users = new Mock<IUserRepository>();
        private Mock<IIdeaRepository> _Ideas = new Mock<IIdeaRepository>();

        public when_an_unauthorized_user_creates_a_new_idea()
        {
            Configure(_Users.Object, _Ideas.Object);
            EnableFormsAuth(_Users);
            Post("/idea/new");
        }

        [Fact]
        public void it_should_redirect_to_the_login_page()
        {
            Response.ShouldHaveRedirectedTo("/login");
        }

        [Fact]
        public void it_should_not_add_a_new_idea_to_the_repository()
        {
            _Ideas.Verify(B => B.Add(It.IsAny<Idea>()), Times.Never());
        }
    }
}