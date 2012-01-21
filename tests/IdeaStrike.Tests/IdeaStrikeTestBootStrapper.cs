using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Ideastrike.Nancy;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Nancy;
using Moq;

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

