using System.Linq;
using Ideastrike.Nancy.Models;
using Nancy;

namespace Ideastrike.Nancy.Modules
{
    public class IdeaModule : NancyModule
    {
        IIdeaRepository IdeaRep = new IdeaRepository();
        public IdeaModule()
        {
            Get["/idea/{id}"] = parameters =>
            {
                var idea = IdeaRep.GetIdea(parameters.id);                
                return string.Format("Id:{0} Title:{1} Description:{2}", idea.Id, idea.Title, idea.Description);                
            };

            Get["/idea/{id}/delete"] = parameters =>
            {
                double id = parameters.id;
                IdeaRep.DeleteIdea(id);
                return string.Format("Deleted Item {0}", id);
            };
        }
    }
}