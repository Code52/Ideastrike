using System;
using System.Linq;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class FeatureModule : NancyModule
    {
        private readonly IIdeaRepository _ideas;
        private readonly IFeatureRepository _features;

        public FeatureModule(IIdeaRepository ideas, IFeatureRepository features)
            : base("/idea")
        {
            _ideas = ideas;
            _features = features;

            Post["/{idea}/feature"] = _ =>
            {
                int id = _.Idea;
                var feature = new Feature
                                {
                                    Time = DateTime.UtcNow,
                                    Text = Request.Form.feature
                                };
                _features.Add(id, feature);

                return Response.AsRedirect(string.Format("/idea/{0}#{1}", id, feature.Id));
            };
        }
    }
}