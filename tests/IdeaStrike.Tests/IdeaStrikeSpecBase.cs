using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Ideastrike;
using Ideastrike.Nancy;
using Ideastrike.Nancy.Models;
using Ideastrike.Nancy.Models.Repositories;
using Moq;
using Nancy;
using Nancy.Responses;
using Nancy.Testing;
using Nancy.ViewEngines;

namespace IdeaStrike.Tests
{
    public class CustomRootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            return Path.GetDirectoryName(typeof(IdeastrikeBootstrapper).Assembly.Location);
        }
    }

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
        protected Mock<IImageRepository> _Images = new Mock<IImageRepository>();

        public IdeaStrikeSpecBase()
        {
            ViewFactory = new Mock<IViewFactory>();

            Bootstrapper = new ConfigurableBootstrapper(with =>
            {
                with.Module<TModule>();
                with.Dependencies(_Users.Object, _Ideas.Object, _Features.Object, _Activity.Object, _Settings.Object, _Images.Object);
                with.DisableAutoRegistration();
                with.NancyEngine<NancyEngine>();
                with.ViewFactory(ViewFactory.Object);
                with.RootPathProvider<CustomRootPathProvider>();
            });
        }

        protected User CreateMockUser(string username)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = username,
                UserClaims = new Collection<UserClaim> { new UserClaim { Claim = new Claim { Name = "admin" } } }
            };
            _Users.Setup(d => d.Get(user.Id)).Returns(user);
            _Users.Setup(d => d.GetUserFromIdentifier(user.Id)).Returns(user);
            _Users.Setup(d => d.FindBy(It.IsAny<Expression<Func<User, bool>>>())).Returns(new[] { user }.AsQueryable());
            return user;
        }

        protected void EnableFormsAuth()
        {
            FormsAuthentication.Enable(Bootstrapper, new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/login",
                UserMapper = _Users.Object
            });
        }

        protected byte[] CreateImageBits()
        {
            var img = new Bitmap(10, 10);
            var imgData = Graphics.FromImage(img);
            imgData.DrawLine(new Pen(Color.Blue), 0, 0, 10, 10);

            byte[] imageBits = null;
            using (var memoryStream = new MemoryStream())
            {
                img.Save(memoryStream, ImageFormat.Bmp);
                imageBits = memoryStream.ToArray();
            }
            return imageBits;
        }

        protected void Get(string path, Action<BrowserContext> browserContext = null)
        {
            Browser = new Browser(Bootstrapper);
            Response = Browser.Get(path, browserContext);
        }

        protected void Post(string path, Action<BrowserContext> browserContext = null)
        {
            Browser = new Browser(Bootstrapper);
            Response = Browser.Post(path, browserContext);
        }

        protected void Put(string path, Action<BrowserContext> browserContext = null)
        {
            Browser = new Browser(Bootstrapper);
            Response = Browser.Put(path, browserContext);
        }

        public Mock<IViewFactory> ViewFactory { get; set; }

        public void SetView(string path, HtmlResponse htmlResponse)
        {
            ViewFactory.Setup(v => v.RenderView(path, It.IsAny<object>(), It.IsAny<ViewLocationContext>()))
                       .Returns(htmlResponse);
        }
    }
}
