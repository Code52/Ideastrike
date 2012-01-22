using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Ideastrike.Nancy.Helpers
{
    public static class ImageExtensions
    {
        public static Image ToThumbnail(this Image image, int desiredHeight)
        {
            var targetWidth = (desiredHeight * image.Width) / image.Height;
            return image.GetThumbnailImage(targetWidth, desiredHeight, () => false, IntPtr.Zero);
        }
    }
}