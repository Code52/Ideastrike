using System;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Ideastrike.Nancy.Modules;
using Moq;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace IdeaStrike.Tests.ApiModuleTests
{
	public class when_posting_a_new_idea : IdeaStrikeSpecBase<ApiSecuredModule>
	{
		public when_posting_a_new_idea() {
			EnableFormsAuth();

			Post("/api/ideas", with => {
				with.JsonBody(new { title = "Test" });
				with.LoggedInUser(CreateMockUser("csainty"));
			});
		}

		[Fact]
		public void it_should_return_created() {
			Assert.Equal(HttpStatusCode.Created, Response.StatusCode);
		}

		[Fact]
		public void it_should_add_the_idea() {
			_Ideas.Verify(d => d.Add(It.IsAny<Idea>()));
		}
	}
}