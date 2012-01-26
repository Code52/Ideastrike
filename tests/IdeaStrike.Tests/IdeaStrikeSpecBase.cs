using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ideastrike.Nancy.Models;
using Moq;
using System.Linq.Expressions;
using Nancy.Testing;
using Ideastrike;
using Nancy;

namespace IdeaStrike.Tests
{
    public class IdeaStrikeSpecBase<TModule> where TModule : NancyModule
    {
        protected ConfigurableBootstrapper Bootstrapper;
        protected Browser Browser;
        protected BrowserResponse Response;

        protected void Configure(params object[] depedencies) {
            Bootstrapper = new ConfigurableBootstrapper(with => {
                with.Module<TModule>();
                with.Dependencies(depedencies);
            });
        }

        protected User CreateMockUser(Mock<IUserRepository> repo, string username) {
            var user = new User {
                Id = Guid.NewGuid(),
                UserName = username
            };
            repo.Setup(d => d.Get(user.Id)).Returns(user);
            repo.Setup(d => d.GetUserFromIdentifier(user.Id)).Returns(user);
            repo.Setup(d => d.FindBy(It.IsAny<Expression<Func<User, bool>>>())).Returns(new[] { user }.AsQueryable());
            return user;
        }

        protected void EnableFormsAuth(Mock<IUserRepository> repo) {
            FormsAuthentication.Enable(Bootstrapper, new FormsAuthenticationConfiguration {
                RedirectUrl = "~/login",
                UserMapper = repo.Object
            });
        }

        protected void Get(string path, Action<BrowserContext> browserContext = null) {
            Browser = new Browser(Bootstrapper);
            Response = Browser.Get(path, browserContext);
        }

        protected void Post(string path, Action<BrowserContext> browserContext = null) {
            Browser = new Browser(Bootstrapper);
            Response = Browser.Post(path, browserContext);
        }

        protected void Put(string path, Action<BrowserContext> browserContext = null) {
            Browser = new Browser(Bootstrapper);
            Response = Browser.Put(path, browserContext);
        }

    }
}
