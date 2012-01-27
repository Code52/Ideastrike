using System;
using System.Data.Entity.Migrations;
using Autofac;
using Ideastrike.Nancy.Helpers;
using Ideastrike.Nancy.Migrations;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using System.Configuration;

namespace Ideastrike.Nancy
{
    public class IdeastrikeBootstrapper : AutofacNancyBootstrapper
    {
        private const string SqlClient = "System.Data.SqlClient";
        protected override void ConfigureRequestContainer(ILifetimeScope existingContainer)
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

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines)
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
            pipelines.OnError.AddItemToEndOfPipeline((context, exception) =>
                                                         {
                                                             var message = string.Format("Exception: {0}", exception);
                                                             new ElmahErrorHandler.LogEvent(message).Raise();
                                                             return null;
                                                         });

            DoMigrations();
        }
    }
}