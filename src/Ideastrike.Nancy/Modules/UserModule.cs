using System;
using Ideastrike.Nancy.Models;
using Nancy;
using Nancy.Security;

namespace Ideastrike.Nancy.Modules
{
    public class UserModule : NancyModule
    {
        public UserModule()
        {
            this.RequiresAuthentication();

            Get["/profile"] = _ =>
            {
                return "Blaaaaaah";
            };
        }
    }
}