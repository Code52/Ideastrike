using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Modules;
using Moq;
using Nancy;
using Xunit;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_creating_a_new_idea : IdeaStrikeSpecBase<IdeaSecuredModule>
    {
        public when_creating_a_new_idea()
        {
            EnableFormsAuth();

            Post("/idea/new", with => with.LoggedInUser(CreateMockUser("shiftkey")));
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


    public class when_creating_a_new_idea_with_no_user : IdeaStrikeSpecBase<IdeaSecuredModule>
    {
        public when_creating_a_new_idea_with_no_user()
        {
            EnableFormsAuth();

            Post("/idea/new", with => with.NoUserSet());
        }

        [Fact]
        public void it_should_redirect_to_the_new_idea()
        {
            Assert.Equal(HttpStatusCode.NotFound, Response.StatusCode);
        }

        [Fact]
        public void it_should_not_a_new_idea_to_the_repository()
        {
            _Ideas.Verify(B => B.Add(It.IsAny<Idea>()), Times.Never());
        }
    }
}