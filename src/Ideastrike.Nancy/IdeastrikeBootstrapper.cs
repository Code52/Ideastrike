using Autofac;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;

namespace Ideastrike.Nancy
{
    public class IdeastrikeBootstrapper : AutofacNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Autofac.ILifetimeScope existingContainer)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<IdeastrikeContext>()
                .AsSelf()
                .SingleInstance();

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

            builder.Update(existingContainer.ComponentRegistry);
        }


        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines)
        {
            // At request startup we modify the request pipelines to
            // include forms authentication - passing in our now request
            // scoped user name mapper.
            //
            // The pipelines passed in here are specific to this request,
            // so we can add/remove/update items in them as we please.
            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/login",
                    UserMapper = container.Resolve<IUserRepository>(),
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }
    }
}