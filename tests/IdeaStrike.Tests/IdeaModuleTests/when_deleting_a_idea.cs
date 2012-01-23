using Xunit;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_deleting_a_idea : IdeaStrikeSpecBase
    {
        public when_deleting_a_idea()
        {
            testResponse = browser.Post("/idea/0/delete/", with => {
                with.LoggedInUser(CreateMockUser("shiftkey"));
            });
        }

        [Fact]
        public void it_should_delete_the_idea_from_the_repository()
        {
            mockIdeasRepo.Verify(B => B.Delete(0));
        }
    }
}

