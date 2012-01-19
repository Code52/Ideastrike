using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class AdminModule : NancyModule
    {
        public AdminModule(IdeastrikeContext dbContext)
            : base("/admin")
        {
            Get["/"] = _ => "";
            Get["/moderation"] = _ => "";
            Get["/search"] = _ => "";
            Get["/forums"] = _ => "";
            Get["/forum/{forumId}"] = _ => "";
            Get["/settings"] = _ => View["Admin/Settings", dbContext.Settings.FirstOrDefault() ?? new Setting()];
            Post["/settings"] = _ =>
                                    {
                                        var settings = dbContext.Settings.FirstOrDefault();
                                        if (settings == null)
                                        {
                                            settings = new Setting();
                                            dbContext.Settings.Add(settings);
                                        }

                                        settings.WelcomeMessage = Request.Form.welcomemessage;
                                        settings.Title = Request.Form.title;
                                        settings.Name = Request.Form.yourname;
                                        settings.HomePage = Request.Form.homepage;
                                        settings.GAnalyticsKey = Request.Form.analyticskey;
                                        /*dbContext.Settings.Add(new Setting
                                                                       {
                                                                           Key = "welcomemessage",
                                                                           Value = Request.Form.welcomemessage
                                                                       });
                                            dbContext.SaveChanges();*/
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