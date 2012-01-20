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
        private IActivityRepository mockActivityRepo = null;
        private IFeatureRepository mockFeatureRepo = null;
        private IIdeaRepository mockIdeasRepo = null;
        private IdeastrikeContext mockIdeaStrikeContext = null;
        private ISettingsRepository mockSettingsRepo = null;

        public IdeaStrikeTestBootStrapper()
        {
            mockActivityRepo = new Mock<IActivityRepository>().Object;
            mockFeatureRepo = new Mock<IFeatureRepository>().Object;
            mockIdeasRepo = new Mock<IIdeaRepository>().Object;
            mockSettingsRepo = new Mock<ISettingsRepository>().Object;
            mockIdeaStrikeContext = new Mock<IdeastrikeContext>().Object;
        }


        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance<ISettingsRepository>(mockSettingsRepo);
            builder.RegisterInstance<IIdeaRepository>(mockIdeasRepo);
            builder.RegisterInstance<IActivityRepository>(mockActivityRepo);
            builder.RegisterInstance<IFeatureRepository>(mockFeatureRepo);
            builder.RegisterInstance<IdeastrikeContext>(mockIdeaStrikeContext);
            builder.Update(existingContainer.ComponentRegistry);
        }
    }
}

