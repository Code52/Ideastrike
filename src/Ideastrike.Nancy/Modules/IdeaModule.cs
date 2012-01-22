using System;
using System.Drawing.Imaging;
using System.IO;
using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Models;
using System.Linq;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaModule : NancyModule
    {
        public IdeaModule(IIdeaRepository ideas, IImageRepository imageRepository)
            : base("/idea")
        {
            Get["/{id}"] = parameters =>
                               {
                                   int id = parameters.id;
                                   Idea idea = ideas.Get(id);
                                   if (idea == null)
                                       return View["Shared/404"];

                                   return View["Idea/Index", idea];

                               };

            Post["/"] = _ =>
            {
                var i = new Idea
                            {
                                Time = DateTime.UtcNow,
                                Title = Request.Form.Title,
                                Description = Request.Form.Description,
                            };


                var form = System.Web.HttpContext.Current.Request.Form;
                //all the images uploaded by the jQuery uploader get uploaded and added to the db ahead of time, 
                //therefore, we also inject a hidden field into the form for every image
                //this way, when we post the actual idea, we have a way to reference back to the image that belongs to the 
                //idea being posted
                i.Images = form.Cast<string>()
                    .Where(k => k.StartsWith("imageId"))
                    .Select(k => imageRepository.Get(Convert.ToInt32(form[k])))
                    .ToList(); //is there a way to do this using Nancy?
                ideas.Add(i);

                return Response.AsRedirect("/idea/" + i.Id);
            };

            Get["/{id}/vote/{userid}"] = parameters =>
            {
                Idea idea = ideas.Get(parameters.id);
                ideas.Vote(idea, parameters.userid, 1);

                return Response.AsJson(new
                                {
                                    Status = "OK",
                                    NewVotes = idea.Votes.Sum(v => v.Value)
                                });
            };

            Get["/{id}/delete"] = parameters =>
            {
                int id = parameters.id;
                ideas.Delete(id);
                return string.Format("Deleted Item {0}", id);
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
                                         if(id.Contains("."))
                                         {
                                             id = id.Substring(0, id.IndexOf(".")); //string .jpg in case it was send in
                                         }
                                         var image = (Image)imageRepository.Get(int.Parse(id));
                                         return Response.FromStream(new MemoryStream(image.ImageBits), "image/jpeg");
                                     };

            Get[@"/imagethumb/{id}/{width}"] = parameters =>
                                          {
                                              
                                              var image = (Image)imageRepository.Get(parameters.id);
                                              using(var memoryStream = new MemoryStream(image.ImageBits))
                                              {
                                                  var drawingImage = System.Drawing.Image.FromStream(memoryStream);
                                                  var thumb = drawingImage.ToThumbnail((int)parameters.width);
                                                  using(var thumbnailStream = new MemoryStream())
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