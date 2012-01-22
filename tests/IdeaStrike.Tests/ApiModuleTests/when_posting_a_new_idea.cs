using System;
using Ideastrike.Nancy.Models;
using Moq;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace IdeaStrike.Tests.ApiModuleTests
{
    public class when_posting_a_new_idea : IdeaStrikeSpecBase
    {
        private BrowserResponse response;
        private User user = new User { Id = Guid.NewGuid() };

        public when_posting_a_new_idea() {
            mockUsersRepo.Setup(d => d.GetUserFromIdentifier(user.Id)).Returns(user);
            response = browser.Post("/api/ideas", with => {
                with.JsonBody(new { title = "Test" });
                with.LoggedInUser(user);
            });
        }

        [Fact]
        public void it_should_return_created() {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public void it_should_add_the_idea() {
            mockIdeasRepo.Verify(d => d.Add(It.IsAny<Idea>()));
        }
    }
}
