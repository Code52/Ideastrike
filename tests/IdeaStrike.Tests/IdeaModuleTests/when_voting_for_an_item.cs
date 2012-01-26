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
    public class when_voting_for_an_item : IdeaStrikeSpecBase<IdeaSecuredModule>
    {
        private Mock<IUserRepository> _Users = new Mock<IUserRepository>();
        private Mock<IIdeaRepository> _Ideas = new Mock<IIdeaRepository>();
        private Idea _Idea = new Idea { Id = 1 };

        public when_voting_for_an_item()
        {
            _Ideas.Setup(d => d.Get(_Idea.Id)).Returns(_Idea);

            Configure(_Users.Object, _Ideas.Object);
            EnableFormsAuth(_Users);

            Post("/idea/1/vote", with => {
                with.LoggedInUser(CreateMockUser(_Users, "shiftkey"));
            });
        }

        [Fact]
        public void it_should_register_the_vote()
        {
            _Ideas.Verify(b => b.Vote(1, It.IsAny<Guid>(), 1));
        }
    }
}

