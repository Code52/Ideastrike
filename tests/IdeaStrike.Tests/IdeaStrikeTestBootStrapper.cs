using System.Collections.Generic;
using System;
using Nancy.Testing;
using Nancy.Bootstrapper;
using Ideastrike;
using Ideastrike.Nancy.Models;

namespace IdeaStrike.Tests
{
    public class IdeaStrikeTestBootStrapper : ConfigurableBootstrapper
    {
        private readonly IDictionary<Type, object> _mocks;
        
        public IdeaStrikeTestBootStrapper(IDictionary<Type,object> mocks)
        {
            _mocks = mocks;
        }

        protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, IPipelines pipelines) {
            FormsAuthentication.Enable(pipelines, new FormsAuthenticationConfiguration {
                RedirectUrl = "~/login",
                UserMapper = _mocks[typeof(IUserRepository)] as IUserRepository
            });
        }

        protected override void ConfigureRequestContainer(TinyIoC.TinyIoCContainer container)
        {
            foreach (var mock in _mocks)
            {
                container.Register(mock.Key, mock.Value);
            }
        }
    }
}

