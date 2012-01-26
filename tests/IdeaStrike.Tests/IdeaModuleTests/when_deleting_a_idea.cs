using Xunit;
using Ideastrike.Nancy.Modules;
using Ideastrike.Nancy.Models.Repositories;
using Moq;
using Ideastrike.Nancy.Models;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_deleting_a_idea : IdeaStrikeSpecBase<IdeaSecuredModule>
    {
        private Mock<IUserRepository> _Users = new Mock<IUserRepository>();
        private Mock<IIdeaRepository> _Ideas = new Mock<IIdeaRepository>();

        public when_deleting_a_idea()
        {
            Configure(_Users.Object, _Ideas.Object);
            EnableFormsAuth(_Users);
            
            Post("/idea/0/delete/", with => {
                with.LoggedInUser(CreateMockUser(_Users, "shiftkey"));
            });
        }

        [Fact]
        public void it_should_delete_the_idea_from_the_repository()
        {
            _Ideas.Verify(B => B.Delete(0));
        }
    }
}

