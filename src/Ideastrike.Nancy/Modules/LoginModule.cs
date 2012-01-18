using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Nancy;
using System.Net;
using Newtonsoft.Json;

namespace Ideastrike.Nancy.Modules
{
    public class LoginModule : NancyModule
    {
        public const string apikey = "c812d525d422befbe8d28539846eff1b0a0f8ed0";

        public LoginModule()
        {
            Get["/login/"] = parameters => View["Login/Index"];

            Post["/login/token"] = x =>
            {
                if(string.IsNullOrWhiteSpace(Request.Form.token)) return "Error retrieving token from Janrain";

                var response = new WebClient().DownloadString(string.Format("https://rpxnow.com/api/v2/auth_info?apiKey={0}&token={1}", apikey, Request.Form.token));

                if(string.IsNullOrWhiteSpace(response)) return "Error retrieving user from Janrain";

                var j = JsonConvert.DeserializeObject<dynamic>(response);

                if (j.stat.ToString() != "ok") return "Invalid response";

                using (var db = new IdeastrikeContext())
                {
                    string userstring = j.profile.identifier.ToString();

                    var userExists = db.Users.FirstOrDefault();

                    if(userExists == null)
                    {
                        var u = new User() {Id = Guid.NewGuid(), Identity = userstring, UserName = "New User"};
                        db.Users.Add(u);
                        db.SaveChanges();
                        return ModuleExtensions.LoginAndRedirect(this, u.Id, DateTime.Now.AddDays(1), "/profile/create");
                    }

                    return ModuleExtensions.Login(this, userExists.Id, DateTime.Now.AddDays(1), "/");
                }
            };

            Get["/logout/"] = parameters =>
                                  {
                                      return ModuleExtensions.LogoutAndRedirect(this, "/");
                                  };
        }
    }
}