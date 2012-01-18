using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy
{
    public class IdeastrikeBootstrapper : DefaultNancyBootstrapper
    {
        /// <summary>
        /// This is called once, when the Bootstrapper is executed, and is used to register dependencies that you 
        /// either wish to have application scope lifetimes (application singletons), or be registered as multi-instance
        /// </summary>
        /// <param name="container">Nancy's TinyIoCContainer</param>
        protected override void ConfigureApplicationContainer(TinyIoC.TinyIoCContainer container)
        {
            container.Register<IdeaRepository>().AsSingleton();
            container.Register<IdeastrikeContext>().AsSingleton();

            base.ConfigureApplicationContainer(container);
        }

        /// <summary>
        /// This is called once per request, before the module matching the route is resolved, and is used to 
        /// register singletons that will have request lifetime.
        /// </summary>
        /// <param name="container">Nancy's TinyIoCContainer</param>
        protected override void ConfigureRequestContainer(TinyIoC.TinyIoCContainer container)
        {
            base.ConfigureRequestContainer(container);
        }
    }
}