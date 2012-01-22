namespace IdeaStrike.Tests.FeatureModuleTests
{
    public class when_viewing_the_feature_page : IdeaStrikeSpecBase
    {
        public when_viewing_the_feature_page()
        {
            var testRequest = PostTestRequest("/idea/0/feature/");
            testResponse = engine.HandleRequest(testRequest).Response;
        }
    }
}
