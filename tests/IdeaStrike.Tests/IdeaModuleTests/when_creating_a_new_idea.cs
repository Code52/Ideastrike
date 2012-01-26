using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nancy;
using Nancy.Security;
using Xunit;
using Moq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Modules;
using Ideastrike.Nancy.Models.Repositories;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_creating_a_new_idea : IdeaStrikeSpecBase<IdeaSecuredModule>
    {
        private Mock<IUserRepository> _Users = new Mock<IUserRepository>();
        private Mock<IIdeaRepository> _Ideas = new Mock<IIdeaRepository>();

        public when_creating_a_new_idea()
        {
            Configure(_Users.Object, _Ideas.Object);
            EnableFormsAuth(_Users);
            
            Post("/idea/new", with => {
                with.LoggedInUser(CreateMockUser(_Users, "shiftkey"));
            });
        }

        [Fact]
        public void it_should_redirect_to_the_new_idea()
        {
            Assert.Equal(HttpStatusCode.SeeOther, Response.StatusCode);
        }

        [Fact]
        public void it_should_add_a_new_idea_to_the_repository()
        {
            _Ideas.Verify(B => B.Add(It.IsAny<Idea>()));
        }
    }
}

