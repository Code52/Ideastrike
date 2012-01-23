using System;
using System.Collections.Generic;
using System.Linq;
using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Nancy.Security;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaSecuredModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;
        private readonly IUserRepository _users;
        private readonly ISettingsRepository _settings;
        private readonly IImageRepository _imageRepository;

        public IdeaSecuredModule(IIdeaRepository ideas, IUserRepository users, ISettingsRepository settings, IImageRepository imageRepository)
            : base("/idea")
        {
            _ideas = ideas;
            _settings = settings;
            _imageRepository = imageRepository;
            _users = users;

            this.RequiresAuthentication();

            Get["/new"] = _ =>
            {
                var m = Context.Model(string.Format("New Idea - {0}", _settings.Title));
                m.Ideas = _ideas.GetAll();
                return View["Idea/New", m];
            };

            Get["/{id}/edit"] = parameters =>
            {
                int id = parameters.id;
                var idea = _ideas.Get(id);

                if (idea == null)
                    return View["404"];

                var m = Context.Model(string.Format("Edit Idea: '{0}' - {1}", idea.Title, _settings.Title));
                m.PopularIdeas = _ideas.GetAll();
                m.Idea = idea;
                m.StatusChoices = _settings.IdeaStatusChoices.Split(',');

                return View["Idea/Edit", m];
            };


            // save result of edit to database
            Post["/{id}/edit"] = parameters =>
            {
                int id = parameters.id;
                var idea = _ideas.Get(id);
                if (idea == null)
                    return View["404"];

                idea.Title = Request.Form.Title;
                idea.Description = Request.Form.Description;
                idea.Status = Request.Form.Status;

                _ideas.Save();

                return Response.AsRedirect(string.Format("/idea/{0}", idea.Id));
            };

            // save result of create to database
            Post["/new"] = _ =>
            {
                var user = _users.FindBy(u => u.UserName == Context.CurrentUser.UserName).FirstOrDefault();

                if (user == null)
                    return Response.AsRedirect("/login");

                var i = new Idea
                            {
                                Author = user,
                                Time = DateTime.UtcNow,
                                Title = Request.Form.Title,
                                Description = Request.Form.Description,
                                Status = settings.IdeaStatusDefault
                            };

                IEnumerable<string> keys = Context.Request.Form;

                var parameters = keys.Where(c => c.StartsWith("imageId"));
                var ids = parameters.Select(c => Context.Request.Form[c].ToString()).Cast<string>();
                var images = ids.Select(id => _imageRepository.Get(Convert.ToInt32(id)));
                i.Images = images.ToList();

                //i.Images = form.Cast<string>()
                //    .Where(k => k.StartsWith("imageId"))
                //    .Select(k => _imageRepository.Get(Convert.ToInt32(form[k])))
                //    .ToList(); //is there a way to do this using Nancy?
                if (i.Votes.Any(u => u.UserId == user.Id))
                    i.UserHasVoted = true;


                ideas.Add(i);

                return Response.AsRedirect("/idea/" + i.Id);
            };

            // someone else votes for the idea
            Post["/{id}/vote"] = parameters =>
            {
                var user = Context.GetCurrentUser(_users);

                if (user == null)
                    return Response.AsRedirect("/login");

                int ideaId = parameters.id;
                int votes = ideas.Vote(ideaId, user.Id, 1);

                return Response.AsJson(new { Status = "OK", NewVotes = votes });
            };

            // the user decides to repeal his vote
            Post["/{id}/unvote"] = parameters =>
            {
                var user = Context.GetCurrentUser(_users);
                int votes = ideas.Unvote(parameters.id, user.Id);

                return Response.AsJson(new { Status = "OK", NewVotes = votes });
            };

            Post["/{id}/delete"] = parameters =>
            {
                int id = parameters.id;
                ideas.Delete(id);
                ideas.Save();

                // TODO: test
                return Response.AsJson(new { Status = "Error" });
            };

            // TODO: do we want unauthenticated users to be allowed to upload posts?
            Post["/uploadimage"] = parameters =>
            {
                var user = Context.GetCurrentUser(_users);
                if (user == null)
                    return Response.AsJson(new { status = "Error" });

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

            Delete["/deleteimage/{id}"] = parameters =>
            {
                var user = Context.GetCurrentUser(_users);
                if (user == null)
                    return Response.AsJson(new { status = "Error" });

                imageRepository.Delete(parameters.id);
                return null;
            };
        }
    }
}