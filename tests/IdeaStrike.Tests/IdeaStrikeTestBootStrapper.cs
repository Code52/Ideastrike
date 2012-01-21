using Autofac;
using Ideastrike.Nancy;
using System;

namespace IdeaStrike.Tests
{
    public class IdeaStrikeTestBootStrapper : IdeastrikeBootstrapper
    {
        private readonly Func<ContainerBuilder> builder;
        public IdeaStrikeTestBootStrapper(Func<ContainerBuilder> builder)
        {
            this.builder = builder;
        }

        protected override void ConfigureRequestContainer(ILifetimeScope existingContainer)
        {
            builder().Update(existingContainer.ComponentRegistry);
        }
    }
}

