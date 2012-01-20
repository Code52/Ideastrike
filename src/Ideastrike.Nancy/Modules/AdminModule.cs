using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class AdminModule : NancyModule
    {
        public AdminModule(IdeastrikeContext dbContext, ISettingsRepository settings)
            : base("/admin")
        {
            Get["/"] = _ => View["Admin/Index", settings];
            Get["/moderation"] = _ => "";
            Get["/search"] = _ => "";
            Get["/forums"] = _ => "";
            Get["/forum/{forumId}"] = _ => "";
            Get["/settings"] = _ => View["Admin/Settings", settings];
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
        }
    }
}