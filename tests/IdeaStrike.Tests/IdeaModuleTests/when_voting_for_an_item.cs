using System;
using Xunit;
using Moq;
using Ideastrike.Nancy.Models;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_voting_for_an_item : IdeaStrikeSpecBase
    {
        public when_voting_for_an_item()
        {
            CreateMockIdea(new Idea { Id= 1 });
            testResponse = browser.Post("/idea/1/vote", with => {
                with.LoggedInUser(CreateMockUser("shiftkey"));
            });
        }

        [Fact]
        public void it_should_register_the_vote()
        {
            mockIdeasRepo.Verify(b => b.Vote(1, It.IsAny<Guid>(), 1));
        }
    }
}

