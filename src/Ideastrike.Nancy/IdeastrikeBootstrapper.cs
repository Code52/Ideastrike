using System;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Threading;
using Autofac;
using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Migrations;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;

namespace Ideastrike.Nancy
{
    public class IdeastrikeBootstrapper : AutofacNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            var builder = new ContainerBuilder();

            if (ConfigurationManager.ConnectionStrings.Count > 0 && ConfigurationManager.ConnectionStrings["Ideastrike"] != null)
                builder.RegisterType<IdeastrikeContext>()
                    .WithParameter(new NamedParameter("nameOrConnectionString", ConfigurationManager.ConnectionStrings["Ideastrike"].ConnectionString + ";MultipleActiveResultSets=true"))
                    .AsSelf()
                    .InstancePerLifetimeScope();

            else
                builder.RegisterType<IdeastrikeContext>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<IdeaRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ActivityRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<FeatureRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<SettingsRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<UserRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ImageRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Update(existingContainer.ComponentRegistry);
        }

        private static void DoMigrations()
        {
            var settings = new IdeastrikeDbConfiguration();
            var migrator = new DbMigrator(settings);
            migrator.Update();
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration
                    {
                        RedirectUrl = "~/login",
                        UserMapper = container.Resolve<IUserRepository>(),
                    };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            pipelines.OnError.AddItemToEndOfPipeline(LogException);
            pipelines.BeforeRequest.AddItemToEndOfPipeline(DetectLanguage);

            DoMigrations();
        }

        private static Response LogException(NancyContext context, Exception exception)
        {
            var message = string.Format("Exception: {0}", exception);
            new ElmahErrorHandler.LogEvent(message).Raise();
            return null;
        }

        private static Response DetectLanguage(NancyContext ctx)
        {
            var lang = ctx.Request.Headers.AcceptLanguage.FirstOrDefault();
            if (lang != null)
            {
                // Accepted language can be something like "fi-FI", but it can also can be like fi-FI,fi;q=0.9,en;q=0.8
                var langId = lang.Item1;
                if (langId.Contains(","))
                {
                    var index = langId.IndexOf(",");
                    langId = langId.Substring(0, index);
                }

                Thread.CurrentThread.CurrentCulture = new CultureInfo(langId);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(langId);
            }
            return null;
        }
    }
}