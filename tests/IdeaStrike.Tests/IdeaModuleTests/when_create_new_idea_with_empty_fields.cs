using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Modules;
using Moq;
using Xunit;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_create_new_idea_with_empty_fields : IdeaStrikeSpecBase<IdeaSecuredModule>
    {
        public when_create_new_idea_with_empty_fields()
        {
            EnableFormsAuth();

            Post("/idea/new", with =>
                                  {
                                      with.LoggedInUser(CreateMockUser("shiftkey"));
                                      with.FormValue("Title", string.Empty);
                                      with.FormValue("Description", string.Empty);
                                  });
        }

        [Fact]
        public void it_should_store_data_to_repository()
        {
            _Ideas.Verify(B => B.Add(It.IsAny<Idea>()), Times.Never());
        }

    }
}
