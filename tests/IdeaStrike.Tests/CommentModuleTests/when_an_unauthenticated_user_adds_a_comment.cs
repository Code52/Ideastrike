using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Modules;
using Moq;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace IdeaStrike.Tests.CommentModuleTests
{
	public class when_an_unauthenticated_user_adds_a_comment : IdeaStrikeSpecBase<CommentModule>
	{
		public when_an_unauthenticated_user_adds_a_comment() {
			EnableFormsAuth();
			Post("/comment/0/add");
		}

		[Fact]
		public void it_should_redirect_to_the_login_page() {
			Response.ShouldHaveRedirectedTo("/login");
		}
	}
}