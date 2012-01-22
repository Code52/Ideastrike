using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ideastrike.Nancy.Models;
using Moq;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace IdeaStrike.Tests.ApiModuleTests
{
    public class when_updating_an_idea : IdeaStrikeSpecBase
    {
        private BrowserResponse response;
        private Idea testIdea = new Idea { Title = "Title", Description = "Description" };

        public when_updating_an_idea()
        {
            mockIdeasRepo.Setup(d => d.Get(1)).Returns(testIdea);
            response = browser.Put("/api/ideas/1", with => with.JsonBody(new { title = "New Title" }));
        }

        [Fact]
        public void it_should_return_created()
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void it_should_update_the_idea()
        {
            Assert.Equal("New Title", testIdea.Title);
            Assert.Equal("Description", testIdea.Description);
            mockIdeasRepo.Verify(d => d.Edit(It.IsAny<Idea>()));
        }
    }
}
