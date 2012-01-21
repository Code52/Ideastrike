using Xunit;
using Moq;
using Ideastrike.Nancy.Models;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_voting_for_an_item : IdeaStrikeSpecBase
    {
        public when_voting_for_an_item()
        {
            var testRequest = PostTestRequest("/idea/0/vote/0");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_register_the_vote()
        {
            mockIdeasRepo.Verify(B => B.Vote(0, 0, 1));
        }
    }
}

