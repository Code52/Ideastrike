using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Modules;
using Moq;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace IdeaStrike.Tests.FeatureModuleTests
{
	public class when_an_unauthenticated_user_adds_a_features : IdeaStrikeSpecBase<FeatureModule>
	{
		public when_an_unauthenticated_user_adds_a_features() {
			EnableFormsAuth();
			Post("/idea/0/feature");
		}

		[Fact]
		public void it_should_redirect_to_the_login_page() {
			Response.ShouldHaveRedirectedTo("/login");
		}
	}
}