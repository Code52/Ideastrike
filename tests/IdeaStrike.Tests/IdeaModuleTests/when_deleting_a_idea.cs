using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Ideastrike.Nancy.Modules;
using Moq;
using Xunit;

namespace IdeaStrike.Tests.IdeaModuleTests
{
	public class when_deleting_a_idea : IdeaStrikeSpecBase<IdeaSecuredModule>
	{
		public when_deleting_a_idea() {
			EnableFormsAuth();

			Post("/idea/0/delete/", with => {
				with.LoggedInUser(CreateMockUser("shiftkey"));
			});
		}

		[Fact]
		public void it_should_delete_the_idea_from_the_repository() {
			_Ideas.Verify(B => B.Delete(0));
		}
	}
}