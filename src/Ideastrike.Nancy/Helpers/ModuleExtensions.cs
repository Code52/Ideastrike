﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ideastrike.Nancy.Models;
using Nancy;
using Nancy.Extensions;
using System.Dynamic;

namespace Ideastrike
{
    public static class ModuleExtensions
    {
        /// <summary>
        /// Logs the user in and returns either an empty 200 response for ajax requests, or a redirect response for non-ajax. <seealso cref="RequestExtensions.IsAjaxRequest"/>
        /// </summary>
        /// <param name="module">Nancy module</param>
        /// <param name="userIdentifier">User identifier guid</param>
        /// <param name="cookieExpiry">Optional expiry date for the cookie (for 'Remember me')</param>
        /// <param name="fallbackRedirectUrl">Url to redirect to if none in the querystring</param>
        /// <returns>Nancy response with redirect if request was not ajax, otherwise with OK.</returns>
        public static Response Login(this NancyModule module, Guid userIdentifier, DateTime? cookieExpiry = null, string fallbackRedirectUrl = "/")
        {
            return module.Context.Request.IsAjaxRequest() ?
                LoginWithoutRedirect(module, userIdentifier, cookieExpiry) :
                LoginAndRedirect(module, userIdentifier, cookieExpiry, fallbackRedirectUrl);
        }

        /// <summary>
        /// Logs the user in with the given user guid and redirects.
        /// </summary>
        /// <param name="module">Nancy module</param>
        /// <param name="userIdentifier">User identifier guid</param>
        /// <param name="cookieExpiry">Optional expiry date for the cookie (for 'Remember me')</param>
        /// <param name="fallbackRedirectUrl">Url to redirect to if none in the querystring</param>
        /// <returns>Nancy response instance</returns>
        public static Response LoginAndRedirect(this NancyModule module, Guid userIdentifier, DateTime? cookieExpiry = null, string fallbackRedirectUrl = "/")
        {
            return FormsAuthentication.UserLoggedInRedirectResponse(module.Context, userIdentifier, cookieExpiry, fallbackRedirectUrl);
        }

        /// <summary>
        /// Logs the user in with the given user guid and returns ok response.
        /// </summary>
        /// <param name="module">Nancy module</param>
        /// <param name="userIdentifier">User identifier guid</param>
        /// <param name="cookieExpiry">Optional expiry date for the cookie (for 'Remember me')</param>
        /// <returns>Nancy response instance</returns>
        public static Response LoginWithoutRedirect(this NancyModule module, Guid userIdentifier, DateTime? cookieExpiry = null)
        {
            return FormsAuthentication.UserLoggedInResponse(userIdentifier, cookieExpiry);
        }

        /// <summary>
        /// Logs the user out and returns either an empty 200 response for ajax requests, or a redirect response for non-ajax. <seealso cref="RequestExtensions.IsAjaxRequest"/>
        /// </summary>
        /// <param name="module">Nancy module</param>
        /// <param name="redirectUrl">URL to redirect to</param>
        /// <returns>Nancy response with redirect if request was not ajax, otherwise with OK.</returns>
        public static Response Logout(this NancyModule module, string redirectUrl)
        {
            return module.Context.Request.IsAjaxRequest() ?
               FormsAuthentication.LogOutResponse() :
               FormsAuthentication.LogOutAndRedirectResponse(module.Context, redirectUrl);
        }

        /// <summary>
        /// Logs the user out and redirects
        /// </summary>
        /// <param name="module">Nancy module</param>
        /// <param name="redirectUrl">URL to redirect to</param>
        /// <returns>Nancy response instance</returns>
        public static Response LogoutAndRedirect(this NancyModule module, string redirectUrl)
        {
            return FormsAuthentication.LogOutAndRedirectResponse(module.Context, redirectUrl);
        }

        public static Response LogoutWithoutRedirect(this NancyModule module)
        {
            return FormsAuthentication.LogOutResponse();
        }

        public static bool IsLoggedIn(this NancyContext context)
        {
            return !(context == null || context.CurrentUser == null ||
                     string.IsNullOrWhiteSpace(context.CurrentUser.UserName));
        }

        public static string Username(this NancyContext context)
        {
            return (context == null || context.CurrentUser == null || string.IsNullOrWhiteSpace(context.CurrentUser.UserName)) ? string.Empty : context.CurrentUser.UserName;
        }

        public static User GetCurrentUser(this NancyContext context, IUserRepository _users)
        {
            if (context == null || context.CurrentUser == null || string.IsNullOrWhiteSpace(context.CurrentUser.UserName))
                return null;

            return _users.FindBy(u => u.UserName == context.CurrentUser.UserName).FirstOrDefault();
        }

        public static dynamic Model(this NancyContext context, string title)
        {
            dynamic model = new ExpandoObject();
            model.Title = title;
            model.IsLoggedIn = context.IsLoggedIn();
            model.UserName = context.Username();
            return model;
        }
    }
}