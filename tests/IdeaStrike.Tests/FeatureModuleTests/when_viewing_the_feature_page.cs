using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Xunit;

namespace IdeaStrike.Tests.FeatureModuleTests
{
    public class when_viewing_the_feature_page : IdeaStrikeSpecBase
    {

        public when_viewing_the_feature_page()
        {
            var testRequest = PostTestRequest("/idea/0/feature/");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_set_the_status_code_to_ok()
        {
            Assert.Equal(HttpStatusCode.SeeOther, testResponse.StatusCode);
        }
    }
}
