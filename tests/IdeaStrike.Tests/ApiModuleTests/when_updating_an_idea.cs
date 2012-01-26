using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ideastrike.Nancy.Models;
using Moq;
using Nancy;
using Nancy.Testing;
using Xunit;
using Ideastrike.Nancy.Modules;
using Ideastrike.Nancy.Models.Repositories;

namespace IdeaStrike.Tests.ApiModuleTests
{
    public class when_updating_an_idea : IdeaStrikeSpecBase<ApiSecuredModule>
    {
        private Idea testIdea = new Idea { Title = "Title", Description = "Description" };
        private Mock<IIdeaRepository> _Ideas = new Mock<IIdeaRepository>();
        private Mock<IUserRepository> _Users = new Mock<IUserRepository>();

        public when_updating_an_idea()
        {
            Configure(_Ideas.Object, _Users.Object);
            EnableFormsAuth(_Users);

            _Ideas.Setup(d => d.Get(1)).Returns(testIdea);
            Put("/api/ideas/1", with => {
                with.JsonBody(new { title = "New Title" });
                with.LoggedInUser(CreateMockUser(_Users, "csainty"));
            });
        }

        [Fact]
        public void it_should_return_created()
        {
            Assert.Equal(HttpStatusCode.OK, Response.StatusCode);
        }

        [Fact]
        public void it_should_update_the_idea()
        {
            Assert.Equal("New Title", testIdea.Title);
            Assert.Equal("Description", testIdea.Description);
            _Ideas.Verify(d => d.Edit(It.IsAny<Idea>()));
        }
    }
}
