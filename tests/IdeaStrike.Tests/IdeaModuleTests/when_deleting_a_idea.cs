using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_deleting_a_idea : IdeaStrikeSpecBase
    {
        public when_deleting_a_idea()
        {
            var testRequest = GetTestRequest("/idea/0/delete/");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_delete_the_idea_from_the_repository()
        {
            mockIdeasRepo.Verify(B => B.Delete(0));
        }
    }
}

