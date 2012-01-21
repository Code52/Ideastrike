using System;
using Xunit;
using Moq;
using Ideastrike.Nancy.Models;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_voting_for_an_item : IdeaStrikeSpecBase
    {
        private Guid ideaGuid = Guid.NewGuid();

        public when_voting_for_an_item()
        {
            var testRequest = GetTestRequest("/idea/0/vote/" + ideaGuid.ToString());
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_register_the_vote()
        {
            mockIdeasRepo.Verify(B => B.Vote(It.IsAny<Idea>().Id, ideaGuid, 1));
        }
    }
}

