using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Xunit;


namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_viewing_the_idea_page : IdeaStrikeSpecBase
    {

        public when_viewing_the_idea_page()
        {
            var testRequest = GetTestRequest("/idea/0/");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_set_the_status_code_to_ok()
        {
            Assert.Equal(HttpStatusCode.OK, testResponse.StatusCode);
        }
    }

    public class and_creating_a_new_idea : IdeaStrikeSpecBase
    {
        
        public and_creating_a_new_idea()
        {
            
            var testRequest = PostTestRequest("/idea/");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        [Fact]
        public void it_should_redirect_to_the_new_idea()
        {
            Assert.Equal(HttpStatusCode.SeeOther, testResponse.StatusCode);
        }
    }

    public class and_voting_for_an_item : IdeaStrikeSpecBase
    {
        
        public and_voting_for_an_item()
        {
            var testRequest = PostTestRequest("/idea/0/vote/0/");
            testResponse = engine.HandleRequest(testRequest).Response;
        }

        public void it_should_register_the_vote()
        {
            
        }
    }

    public class and_deleting_a_idea : IdeaStrikeSpecBase
    {
        
        public and_deleting_a_idea()
        {
            var testRequest = GetTestRequest("/idea/0/delete/");
            testResponse = engine.HandleRequest(testRequest).Response;          
        }

        public void it_should_delete_the_idea()
        {
            
        }
    }
}
