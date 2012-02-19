using System;
using System.Linq;
using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Localization;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Nancy.Security;

namespace Ideastrike.Nancy.Modules
{
    public class ProfileModule : NancyModule
    {
        public readonly IUserRepository _users;
        public readonly IFeatureRepository _features;
        public readonly IIdeaRepository _ideas;

        public ProfileModule(IUserRepository users, IIdeaRepository ideas, IFeatureRepository features)
        {
            _users = users;
            _ideas = ideas;
            _features = features;

            this.RequiresAuthentication();

            Get["/profile"] = _ =>
                                  {
                                      User user = Context.GetCurrentUser(_users);
                                      if (user == null) return Response.AsRedirect("/");

                                      var usersIdeas = _ideas.GetAll().Where(u => u.Author.Id == user.Id).ToList();
                                      var usersFeatures = _features.GetAll().Where(u => u.User.Id == user.Id).ToList();
                                      var usersVotes = _users.GetVotes(user.Id).ToList();

                                      return View["Profile/Index",
                                          new
                                          {
                                              Title = "Profile", 
                                              Id = user.Id, 
                                              UserName = user.UserName, 
                                              Email = user.Email, 
                                              Github = user.Github,
                                              Ideas = usersIdeas,
                                              Features = usersFeatures,
                                              Votes = usersVotes,
                                              AvatarUrl = user.AvatarUrl,
                                              Claims = user.Claims.ToList(),
                                              IsLoggedIn = Context.IsLoggedIn()
                                          }];
                                  };

            Get["/profile/edit"] = _ =>
                                       {
                                           User user = Context.GetCurrentUser(_users);
                                           if (user == null) return Response.AsRedirect("/");


                                           return View["Profile/Edit", new
                                                                           {
                                                                               Title = "Profile", 
                                                                               Id = user.Id, 
                                                                               UserName = user.UserName, 
                                                                               Email = user.Email, 
                                                                               Github = user.Github,
                                                                               Claims = user.Claims.ToList(),
                                                                               IsLoggedIn = Context.IsLoggedIn(),
                                                                           }];
                                       };

            Post["/profile/checkuser"] = _ =>
                                             {
                                                 string username = Request.Form.username;

                                                 var userExists = _users.FindBy(u => u.UserName == username).Any();

                                                 string msg = "";

                                                 if (username == Context.CurrentUser.UserName)
                                                     msg = "";
                                                 else if (string.IsNullOrWhiteSpace(username))
                                                     msg = Strings.UserModule_UsernameNotValid;
                                                 else if (userExists)
                                                     msg = Strings.UserModule_UsernameTaken;
                                                 else msg = Strings.UserModule_UsernameAvailable;

                                                 return Response.AsJson(new
                                                                            {
                                                                                Status = "OK",
                                                                                msg = msg
                                                                            });
                                             };

            Post["/profile/save"] = _ =>
                                        {
                                            var user = Context.GetCurrentUser(_users);
                                            user.UserName = Request.Form.username;
                                            user.Email = Request.Form.email;
                                            user.AvatarUrl = user.Email.ToGravatarUrl(40);
                                            user.Github = Request.Form.github;

                                            _users.Edit(user);

                                            return Response.AsRedirect("/profile");
                                        };
        }
    }
}