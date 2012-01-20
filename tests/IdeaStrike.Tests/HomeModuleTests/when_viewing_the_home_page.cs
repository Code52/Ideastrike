using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using Nancy;
using Ideastrike.Nancy.Modules;
using Ideastrike.Nancy.Models.Repositories;
using Ideastrike.Nancy.Models;


namespace IdeaStrike.Tests.HomeModuleTests
{
    public class when_viewing_the_home_page : IdeaStrikeSpecBase
    {
        
        public when_viewing_the_home_page()
        {
            var testRequest = GetTestRequest("/");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_set_the_status_code_to_ok()
        {
            Assert.Equal(HttpStatusCode.OK, testResponse.StatusCode);
        }

        private static Request GetTestRequest(string route)
        {
            return new Request("GET", route, "http");
        }
    }
}
