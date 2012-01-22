using System;
using System.Dynamic;
using System.Linq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Nancy.Security;

namespace Ideastrike.Nancy.Modules
{
    public class AdminModule : NancyModule
    {
        public AdminModule(IdeastrikeContext dbContext, ISettingsRepository settings, IUserRepository users)
            : base("/admin")
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.Title));
                m.Name = settings.Name;
                m.WelcomeMessage = settings.WelcomeMessage;
                m.HomePage = settings.HomePage;
                m.GAnalyticsKey = settings.GAnalyticsKey;
                return View["Admin/Index", m];
            };

            Get["/users"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.Title));
                m.Name = settings.Name;
                m.WelcomeMessage = settings.WelcomeMessage;
                m.HomePage = settings.HomePage;
                m.GAnalyticsKey = settings.GAnalyticsKey;
                m.Users = users.GetAll();
                return View["Admin/Users", m];
            };

            Get["/moderation"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.Title));
                m.Name = settings.Name;
                m.WelcomeMessage = settings.WelcomeMessage;
                m.HomePage = settings.HomePage;
                m.GAnalyticsKey = settings.GAnalyticsKey;
                return View["Admin/Moderation", m];
            };

            Get["/settings"] = _ =>
            {
                var m = Context.Model(string.Format("Admin - {0}", settings.Title));
                m.Name = settings.Name;
                m.WelcomeMessage = settings.WelcomeMessage;
                m.HomePage = settings.HomePage;
                m.GAnalyticsKey = settings.GAnalyticsKey;

                return View["Admin/Settings", m];
            };

            Post["/settings"] = _ =>
            {
                settings.WelcomeMessage = Request.Form.welcomemessage;
                settings.Title = Request.Form.title;
                settings.Name = Request.Form.yourname;
                settings.HomePage = Request.Form.homepage;
                settings.GAnalyticsKey = Request.Form.analyticskey;
                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    var y = ex.Message;
                }

                return Response.AsRedirect("/admin/settings");
            };

            Get["/search"] = _ => "";
            Get["/forums"] = _ => "";
            Get["/forum/{forumId}"] = _ => "";
        }
    }
}