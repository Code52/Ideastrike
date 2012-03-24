using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Modules;
using Moq;
using Nancy;
using Xunit;

namespace IdeaStrike.Tests.IdeaModuleTests
{
	public class when_an_unauthorized_user_creates_a_new_idea : IdeaStrikeSpecBase<IdeaSecuredModule>
	{
		public when_an_unauthorized_user_creates_a_new_idea() {
			EnableFormsAuth();
			Post("/idea/new");
		}

		[Fact]
		public void it_should_redirect_to_the_login_page() {
            Assert.Equal(HttpStatusCode.NotFound, Response.StatusCode);
		}

		[Fact]
		public void it_should_not_add_a_new_idea_to_the_repository() {
			_Ideas.Verify(B => B.Add(It.IsAny<Idea>()), Times.Never());
		}
	}
}