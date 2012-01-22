using System.Drawing.Imaging;
using System.IO;
using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Ideastrike.Nancy.Models.ViewModels;
using Nancy;
using Nancy.ViewEngines;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Nancy.Helpers;
using Nancy.Extensions;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;
        private readonly ISettingsRepository _settings;

        public IdeaModule(IIdeaRepository ideas, ISettingsRepository settings, IImageRepository imageRepository)
            : base("/idea")
        {
            _ideas = ideas;
            _settings = settings;

            Get["/{id}"] = parameters =>
            {
                int id = parameters.id;
                var idea = _ideas.Get(id);
                if (idea == null)
                    return View["404"];

                var viewModel = new IdeaViewModel(idea);

                dynamic model = new
                {
                    Title = string.Format("{0} - {1}", idea.Title, _settings.Title),
                    Idea = viewModel,
                    IsLoggedIn = Context.IsLoggedIn(),
                    UserName = Context.Username()
                };

                return View["Idea/Index", model];
            };

            Get["/image/{id}"] = parameters =>
            {
                var id = (string)parameters.id;
                if (id.Contains("."))
                {
                    id = id.Substring(0, id.IndexOf(".")); //string .jpg in case it was send in
                }
                var image = imageRepository.Get(int.Parse(id));
                // TODO: format should be adaptive based on backing source?
                return Response.FromStream(new MemoryStream(image.ImageBits), "image/jpeg");
            };

            Get[@"/imagethumb/{id}/{width}"] = parameters =>
            {
                var image = (Image)imageRepository.Get(parameters.id);
                using (var memoryStream = new MemoryStream(image.ImageBits))
                {
                    var drawingImage = System.Drawing.Image.FromStream(memoryStream);
                    var thumb = drawingImage.ToThumbnail((int)parameters.width);
                    using (var thumbnailStream = new MemoryStream())
                    {
                        // TODO: format should be adaptive based on backing source?
                        thumb.Save(thumbnailStream, ImageFormat.Jpeg);
                        return Response.FromStream(new MemoryStream(thumbnailStream.GetBuffer()), "image/jpeg"); //massive WTF? If I just use thumnailStream, it doesn't work...
                    }
                }
            };
        }
    }
}