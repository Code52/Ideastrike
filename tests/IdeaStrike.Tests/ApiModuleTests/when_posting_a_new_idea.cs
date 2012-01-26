using System;
using Ideastrike.Nancy.Models;
using Moq;
using Nancy;
using Nancy.Testing;
using Xunit;
using Ideastrike.Nancy.Modules;
using Ideastrike.Nancy.Models.Repositories;

namespace IdeaStrike.Tests.ApiModuleTests
{
    public class when_posting_a_new_idea : IdeaStrikeSpecBase<ApiSecuredModule>
    {
        private Mock<IUserRepository> _Users = new Mock<IUserRepository>();
        private Mock<IIdeaRepository> _Ideas = new Mock<IIdeaRepository>();

        public when_posting_a_new_idea() {
            Configure(_Users.Object, _Ideas.Object);
            EnableFormsAuth(_Users);

            Post("/api/ideas", with => {
                with.JsonBody(new { title = "Test" });
                with.LoggedInUser(CreateMockUser(_Users, "csainty"));
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
