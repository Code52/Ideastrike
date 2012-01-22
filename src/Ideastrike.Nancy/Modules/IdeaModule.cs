using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Ideastrike.Nancy.Models.ViewModels;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;
        private readonly IUserRepository _users;
        private readonly ISettingsRepository _settings;
        public IdeaModule(IIdeaRepository ideas, IUserRepository users, ISettingsRepository settings, IImageRepository imageRepository)
            : base("/idea")
        {
            _ideas = ideas;
            _settings = settings;
            _users = users;

            Get["/{id}"] = parameters =>
            {
                int id = parameters.id;
                var idea = _ideas.Get(id);
                if (idea == null)
                    return View["404"];

                User user = Context.GetCurrentUser(_users);
                if (user != null)
                {
                    if (idea.Votes.Any(u => u.UserId == user.Id))
                        idea.UserHasVoted = true;

                }

                var viewModel = new IdeaViewModel(idea);
                var model = Context.Model(string.Format("{0} - {1}", idea.Title, _settings.Title));
                model.Idea = viewModel;
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