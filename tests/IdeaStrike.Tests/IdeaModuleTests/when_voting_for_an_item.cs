using System;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Modules;
using Moq;
using Xunit;

namespace IdeaStrike.Tests.IdeaModuleTests
{
	public class when_voting_for_an_item : IdeaStrikeSpecBase<IdeaSecuredModule>
	{
		private Idea _Idea = new Idea { Id = 1 };

		public when_voting_for_an_item() {
			_Ideas.Setup(d => d.Get(_Idea.Id)).Returns(_Idea);
			EnableFormsAuth();

			Post("/idea/1/vote", with => with.LoggedInUser(CreateMockUser("shiftkey")));
		}

		[Fact]
		public void it_should_register_the_vote() {
			_Ideas.Verify(b => b.Vote(1, It.IsAny<Guid>(), 1));
		}
	}
}
