using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Vantage.Common.Models;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;
        public AuthenticationService(IConfiguration iConfig)
        {
            _configuration = iConfig;
            _apiBaseUrl = _configuration.GetSection("ApiConfig").GetSection("BaseUrl").Value;
        }

        public async Task<UserReturnObject> AuthenticateUser(string username, string clearTextPassword)
        {
            var hashedPassword = Helpers.Helper.GenerateSHA256String(clearTextPassword);
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
            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Add("Key", "Secret@123");
                StringContent content = new StringContent(JsonConvert.SerializeObject(userAuthentication), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{_apiBaseUrl}/api/users/authenticate", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    try
                    {
                        userReturnObject = JsonConvert.DeserializeObject<UserReturnObject>(apiResponse);
                    }
                    catch (Exception ex)
                    {
                        //ViewBag.Result = apiResponse;
                        //return View();
                    }
                }
            }
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
