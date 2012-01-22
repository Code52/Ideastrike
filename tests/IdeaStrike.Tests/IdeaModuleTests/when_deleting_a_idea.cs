using Moq;
using Nancy;
using Nancy.Security;
using Xunit;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_deleting_a_idea : IdeaStrikeSpecBase
    {
        public when_deleting_a_idea()
        {
            var testRequest = PostTestRequest("/idea/0/delete/");
            RunBefore(AuthenticateUser);
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        private static Response AuthenticateUser(NancyContext arg)
        {
            var user = new Mock<IUserIdentity>();
            user.Setup(i => i.UserName).Returns("shiftkey");
            arg.CurrentUser = user.Object;
            return null;
        }

        [Fact]
        public void it_should_delete_the_idea_from_the_repository()
        {
            mockIdeasRepo.Verify(B => B.Delete(0));
        }
    }
}

