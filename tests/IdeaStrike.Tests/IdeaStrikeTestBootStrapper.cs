using Autofac;
using Ideastrike.Nancy;

namespace IdeaStrike.Tests
{
    public class IdeaStrikeTestBootStrapper : IdeastrikeBootstrapper
    {
        private readonly ContainerBuilder builder;
        public IdeaStrikeTestBootStrapper(ContainerBuilder builder)
        {
            this.builder = builder;
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            builder.Update(existingContainer.ComponentRegistry);
        }
    }
}

