using Ideastrike.Nancy.Models;
using Moq;
using Xunit;
using Ideastrike.Nancy.Modules;

namespace IdeaStrike.Tests.FeatureModuleTests
{
    // TODO: test that unauthenticated user cannot access resource

    public class when_adding_a_new_feature : IdeaStrikeSpecBase<FeatureModule>
    {
        private Mock<IUserRepository> _Users = new Mock<IUserRepository>();
        private Mock<IFeatureRepository> _Features = new Mock<IFeatureRepository>();

        public when_adding_a_new_feature()
        {
            Configure(_Users.Object, _Features.Object);
            EnableFormsAuth(_Users);
            Post("/idea/0/feature", with => {
                with.LoggedInUser(CreateMockUser(_Users, "shiftkey"));
            });
        }

        [Fact]
        public void it_should_add_the_new_feature()
        {
            _Features.Verify(B => B.Add(0,It.IsAny<Feature>()));
        }
    }
}