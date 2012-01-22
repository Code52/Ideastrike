using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Ideastrike.Nancy.Helpers
{
    public static class ImageExtensions
    {
        public static Image ToThumbnail(this Image image, int desiredWidth)
        {
            var targetHeight = (desiredWidth * image.Height) / image.Width;
            return image.GetThumbnailImage(desiredWidth, targetHeight, () => false, IntPtr.Zero);
        }
    }
}