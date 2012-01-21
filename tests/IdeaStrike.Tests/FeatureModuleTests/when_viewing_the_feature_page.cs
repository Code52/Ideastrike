using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Xunit;
using Moq;
using Ideastrike.Nancy.Models;

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

    public class when_adding_a_new_feature : IdeaStrikeSpecBase
    {
        
        public when_adding_a_new_feature()
        {
            var testRequest = PostTestRequest("/idea/0/feature");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_add_the_new_feature()
        {
            mockFeatureRepo.Verify(B => B.Add(0,It.IsAny<Feature>()));
        }
    }
}
