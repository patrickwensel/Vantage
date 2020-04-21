using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Vantage.Common.Models;
using Vantage.Common.Utility;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.Services
{
    public class AuthenticationService : BaseAPIService, IAuthenticationService
    {
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;
        public AuthenticationService(IConfiguration iConfig)
        {
            _configuration = iConfig;
            _apiBaseUrl = Config.GetAPIBaseUrl(_configuration);
            SetBaseUrlAndTimeout(_apiBaseUrl);
        }

        public async Task<UserReturnObject> AuthenticateUser(string username, string clearTextPassword)
        {
            string hashedPassword = null;
            if (!string.IsNullOrEmpty(clearTextPassword))
                hashedPassword = Helpers.Helper.GenerateSHA256String(clearTextPassword);

            UserAuthentication userAuthentication = new UserAuthentication
            {
                UserName = username,
                Password = hashedPassword
            };

            UserReturnObject userReturnObject = await Authenticate(userAuthentication);

            return userReturnObject;
        }

        private async Task<UserReturnObject> Authenticate(UserAuthentication userAuthentication)
        {
            UserReturnObject userReturnObject = new UserReturnObject();
            userReturnObject = await PostRequest<UserReturnObject>("/api/users/authenticate", userAuthentication);
            return userReturnObject;
        }

        private string CalculateHash(string clearTextPassword, string salt)
        {
            // Convert the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
            // Use the hash algorithm to calculate the hash
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // Return the hash as a base64 encoded string to be compared to the stored password
            return Convert.ToBase64String(hash);
        }
    }
}
