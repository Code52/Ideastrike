using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ideastrike.Nancy.Modules;
using Xunit;
using Ideastrike.Nancy.Models;

namespace IdeaStrike.Tests.IdeaModuleTests
{
    public class when_viewing_an_image_thumbnail_with_width_less_than_max : IdeaStrikeSpecBase<IdeaModule>
    {
        private Image testImage;

        public when_viewing_an_image_thumbnail_with_width_less_than_max()
        {
            testImage = new Image
            {
                Id = 1,
                Name = "Test Image",
                ImageBits = CreateImageBits(),
                IdeaId = 1
            };
            _Images.Setup(i => i.Get(1)).Returns(testImage);
            _Settings.SetupGet(s => s.MaxThumbnailWidth).Returns(1000);
            Get("/idea/imagethumb/1/500");
        }

        [Fact]
        public void it_should_return_status_ok()
        {
            Assert.Equal(Nancy.HttpStatusCode.OK, Response.StatusCode);
        }
    }
}
