using Autofac;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Ideastrike.Nancy.Modules;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System;

namespace Ideastrike.Nancy
{
    public class IdeastrikeBootstrapper : AutofacNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(Autofac.ILifetimeScope existingContainer)
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
    }
}