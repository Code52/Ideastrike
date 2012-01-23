using Ideastrike;
using Ideastrike.Nancy.Models;
using Nancy.Testing;

namespace IdeaStrike.Tests
{
    public static class BrowserContextExtensions
    {
        public static void LoggedInUser(this BrowserContext ctx, User user) {
            var cookie = FormsAuthentication.UserLoggedInResponse(user.Id).Cookies[0].Value;
            ctx.Cookie(FormsAuthentication.FormsAuthenticationCookieName, cookie);
        }
    }
}
