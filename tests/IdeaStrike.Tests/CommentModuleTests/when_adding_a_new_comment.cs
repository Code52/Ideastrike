using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ideastrike.Nancy.Models;
using Moq;
using Xunit;
using Ideastrike.Nancy.Modules;
using Ideastrike.Nancy.Models.Repositories;

namespace IdeaStrike.Tests.CommentModuleTests
{
    // TODO: test that an unauthenticated user doesn't get access to this resource

    public class when_adding_a_new_comment : IdeaStrikeSpecBase<CommentModule>
    {
        private Mock<IUserRepository> _Users = new Mock<IUserRepository>();
        private Mock<IActivityRepository> _Activity = new Mock<IActivityRepository>();

        public when_adding_a_new_comment()
        {
            Configure(_Users.Object, _Activity.Object);
            EnableFormsAuth(_Users);
            Post("/comment/0/add", with => {
                with.FormValue("userId", "1");
                with.FormValue("comment", "words");
                with.LoggedInUser(CreateMockUser(_Users, "shiftkey"));
            });
        }

        [Fact]
        public void it_should_add_the_comment()
        {
            _Activity.Verify(B => B.Add(0, It.IsAny<Comment>()));
        }
    }
}

