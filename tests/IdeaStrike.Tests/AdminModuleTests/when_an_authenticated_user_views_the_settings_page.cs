using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ideastrike.Nancy.Modules;
using Xunit;
using Nancy.Testing;

namespace IdeaStrike.Tests.AdminModuleTests
{
    public class when_an_authenticated_user_views_the_settings_page : IdeaStrikeSpecBase<AdminModule>
    {
        public when_an_authenticated_user_views_the_settings_page()
        {
            EnableFormsAuth();

            Get("/admin/settings", with =>
            {
                with.LoggedInUser(CreateMockUser("shiftkey"));
            }); 
        }

        public void it_includes_the_max_thumbnail_width_field()
        {
            Response.Body["#maxthumbnailwidth"].ShouldExistOnce();
        }
    }
}
