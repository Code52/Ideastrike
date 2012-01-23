using System.Collections.Generic;
using System;
using Nancy.Testing;

namespace IdeaStrike.Tests
{
    public class IdeaStrikeTestBootStrapper : ConfigurableBootstrapper
    {
        private readonly IDictionary<Type, object> _mocks;
        
        public IdeaStrikeTestBootStrapper(IDictionary<Type,object> mocks)
        {
            _mocks = mocks;
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

