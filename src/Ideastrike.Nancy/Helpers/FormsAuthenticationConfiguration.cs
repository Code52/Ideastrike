using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ideastrike.Nancy.Models;
using Nancy.Authentication.Forms;
using Nancy.Cryptography;

namespace Ideastrike
{
    public class FormsAuthenticationConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormsAuthenticationConfiguration"/> class.
        /// </summary>
        public FormsAuthenticationConfiguration()
            : this(CryptographyConfiguration.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormsAuthenticationConfiguration"/> class.
        /// </summary>
        /// <param name="cryptographyConfiguration">Cryptography configuration</param>
        public FormsAuthenticationConfiguration(CryptographyConfiguration cryptographyConfiguration)
        {
            CryptographyConfiguration = cryptographyConfiguration;
        }

        /// <summary>
        /// Gets or sets the redirect url for pages that require authentication
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the username/identifier mapper
        /// </summary>
        public IUserRepository UserMapper { get; set; }

        /// <summary>
        /// Gets or sets the cryptography configuration
        /// </summary>
        public CryptographyConfiguration CryptographyConfiguration { get; set; }

        /// <summary>
        /// Gets a value indicating whether the configuration is valid or not.
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(RedirectUrl))
                {
                    return false;
                }

                if (UserMapper == null)
                {
                    return false;
                }

                if (CryptographyConfiguration == null)
                {
                    return false;
                }

                if (CryptographyConfiguration.EncryptionProvider == null)
                {
                    return false;
                }

                if (CryptographyConfiguration.HmacProvider == null)
                {
                    return false;
                }

                return true;
            }
        }
    }
}