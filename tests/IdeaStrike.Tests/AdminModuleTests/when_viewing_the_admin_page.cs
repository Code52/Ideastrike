using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Xunit;


namespace IdeaStrike.Tests.AdminModuleTests
{
    public class when_viewing_the_admin_page : IdeaStrikeSpecBase
    {

        public when_viewing_the_admin_page()
        {
            var testRequest = GetTestRequest("/admin");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_set_the_status_code_to_ok()
        {
            Assert.Equal(HttpStatusCode.OK, testResponse.StatusCode);
        }
    }
}
