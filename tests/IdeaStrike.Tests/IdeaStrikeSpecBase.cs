using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Ideastrike;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Moq;
using Nancy;
using Nancy.Testing;

namespace IdeaStrike.Tests
{
	public class IdeaStrikeSpecBase<TModule> where TModule : NancyModule
	{
		protected ConfigurableBootstrapper Bootstrapper;
		protected Browser Browser;
		protected BrowserResponse Response;

		protected Mock<IUserRepository> _Users = new Mock<IUserRepository>();
		protected Mock<IIdeaRepository> _Ideas = new Mock<IIdeaRepository>();
		protected Mock<IFeatureRepository> _Features = new Mock<IFeatureRepository>();
		protected Mock<IActivityRepository> _Activity = new Mock<IActivityRepository>();
		protected Mock<ISettingsRepository> _Settings = new Mock<ISettingsRepository>();
		protected Mock<IdeastrikeContext> _Context = new Mock<IdeastrikeContext>();
		protected Mock<IImageRepository> _Images = new Mock<IImageRepository>();

		public IdeaStrikeSpecBase() {
			Bootstrapper = new ConfigurableBootstrapper(with => {
				with.Module<TModule>();
				with.Dependencies(_Users.Object, _Ideas.Object, _Features.Object, _Activity.Object, _Settings.Object, _Context.Object, _Images.Object);
				with.DisableAutoRegistration();
				with.NancyEngine<NancyEngine>();
			});
		}

		protected User CreateMockUser(string username) {
			var user = new User {
				Id = Guid.NewGuid(),
				UserName = username
			};
			_Users.Setup(d => d.Get(user.Id)).Returns(user);
			_Users.Setup(d => d.GetUserFromIdentifier(user.Id)).Returns(user);
			_Users.Setup(d => d.FindBy(It.IsAny<Expression<Func<User, bool>>>())).Returns(new[] { user }.AsQueryable());
			return user;
		}

		protected void EnableFormsAuth() {
			FormsAuthentication.Enable(Bootstrapper, new FormsAuthenticationConfiguration {
				RedirectUrl = "~/login",
				UserMapper = _Users.Object
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