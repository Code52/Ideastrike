using System;
using System.Linq;
using System.Drawing.Imaging;
using System.IO;
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

                    var form = System.Web.HttpContext.Current.Request.Form;
                    //all the images uploaded by the jQuery uploader get uploaded and added to the db ahead of time, 
                    //therefore, we also inject a hidden field into the form for every image
                    //this way, when we post the actual idea, we have a way to reference back to the image that belongs to the 
                    //idea being posted
                    idea.Images = form.Cast<string>()
                        .Where(k => k.StartsWith("imageId"))
                        .Select(k => imageRepository.Get(Convert.ToInt32(form[k])))
                        .ToList(); //is there a way to do this using Nancy?
                    if (idea.Votes.Any(u => u.UserId == user.Id))
                        idea.UserHasVoted = true;

                }

                var viewModel = new IdeaViewModel(idea);

                dynamic model = new
                {
                    Title = string.Format("{0} - {1}", idea.Title, _settings.Title),
                    Idea = viewModel,
                    IsLoggedIn = Context.IsLoggedIn(),
                    UserName = Context.Username(),
                };

                return View["Idea/Index", model];
            };

            Post["/uploadimage"] = parameters =>
                                       {
                                           var imageFile = Request.Files.FirstOrDefault();
                                           if (imageFile == null)
                                           {
                                               return null; //TODO: handle error case
                                           }

                                           var image = new Image { Name = imageFile.Name };
                                           var bytes = new byte[imageFile.Value.Length];
                                           imageFile.Value.Read(bytes, 0, bytes.Length);
                                           image.ImageBits = bytes;
                                           imageRepository.Add(image);
                                           var status = new ImageFileStatus(image.Id, bytes.Length, image.Name);
                                           return Response.AsJson(new[] { status }).WithHeader("Vary", "Accept");

                                       };
            Get["/image/{id}"] = parameters =>
                                     {
                                         var id = (string)parameters.id;
                                         if (id.Contains("."))
                                         {
                                             id = id.Substring(0, id.IndexOf(".")); //string .jpg in case it was send in
                                         }
                                         var image = (Image)imageRepository.Get(int.Parse(id));
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
                                                      thumb.Save(thumbnailStream, ImageFormat.Jpeg);
                                                      return Response.FromStream(new MemoryStream(thumbnailStream.GetBuffer()), "image/jpeg"); //massive WTF? If I just use thumnailStream, it doesn't work...
                                                  }
                                              }






                                          };

            Delete["/deleteimage/{id}"] = parameters =>
                                          {
                                              imageRepository.Delete(parameters.id);
                                              return null;
                                          };
        }

    }
}