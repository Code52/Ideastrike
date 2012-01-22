using Xunit;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_deleting_a_idea : IdeaStrikeSpecBase
    {
        public when_deleting_a_idea()
        {
            var testRequest = PostTestRequest("/idea/0/delete/");
            RunFirst(r => AuthenticateUser(r, "shiftkey"));
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_delete_the_idea_from_the_repository()
        {
            mockIdeasRepo.Verify(B => B.Delete(0));
        }
    }
}

