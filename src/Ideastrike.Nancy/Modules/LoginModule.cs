using System;
using System.Linq;
using System.Web;
using Ideastrike.Nancy.Models;
using Nancy;
using System.Net;
using Newtonsoft.Json;

namespace Ideastrike.Nancy.Modules
{
    public class LoginModule : NancyModule
    {
        public const string apikey = "46ad5595cb74064655718a434126ef9f11a51a70";

        private IUserRepository _user;

        public LoginModule(IUserRepository userRepository)
        {
            _user = userRepository;

            Post["/login/token"] = x =>
                                       {
                                           if (string.IsNullOrWhiteSpace(Request.Form.token))
                                               return
                                                   View[
                                                       "Login/Error",
                                                       new
                                                           {
                                                               Title = "Login Error",
                                                               Message =
                                                           "Bad response from login provider - could not find login token."
                                                           }];

                                           var response =
                                               new WebClient().DownloadString(
                                                   string.Format(
                                                       "https://rpxnow.com/api/v2/auth_info?apiKey={0}&token={1}",
                                                       apikey, Request.Form.token));

                                           if (string.IsNullOrWhiteSpace(response))
                                               return
                                                   View[
                                                       "Login/Error",
                                                       new
                                                           {
                                                               Title = "Login Error",
                                                               Message =
                                                           "Bad response from login provider - could not find user."
                                                           }];

                                           var j = JsonConvert.DeserializeObject<dynamic>(response);

                                           if (j.stat.ToString() != "ok")
                                               return
                                                   View[
                                                       "Login/Error",
                                                       new
                                                           {
                                                               Title = "Login Error",
                                                               Message = "Bad response from login provider."
                                                           }];

                                           var userIdentity = j.profile.identifier.ToString();

                                           var user = _user.GetUserFromUserIdentity(userIdentity);

                                           if (user == null)
                                           {
                                               var u = new User
                                                           {
                                                               Id = Guid.NewGuid(),
                                                               Identity = userIdentity,
                                                               UserName = "New User",
                                                               IsDeleted = true
                                                           };
                                               _user.Add(u);
                                               return ModuleExtensions.LoginAndRedirect(this, u.Id,
                                                                                        DateTime.Now.AddDays(1),
                                                                                        "/profile/create");
                                           }

                                           return ModuleExtensions.Login(this, user.Id, DateTime.Now.AddDays(1), "/");
                                       };

            Get["/logout/"] = parameters =>
                                  {
                                      return ModuleExtensions.LogoutAndRedirect(this, "/");
                                  };
        }
    }
}