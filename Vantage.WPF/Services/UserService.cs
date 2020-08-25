using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vantage.Common.Models;
using Vantage.Common.Utility;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.Services
{
    public class UserService : BaseAPIService, IUserService
    {
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration iConfig)
        {
            _configuration = iConfig;
            _apiBaseUrl = Config.GetAPIBaseUrl(_configuration);
            SetBaseUrlAndTimeout(_apiBaseUrl);
        }

        public async Task<IList<User>> GetUsers()
        {
            var users = await GetRequest<IList<User>>($"api/Users");
            return users;
        }

        public async Task UpdateCredential(User user)
        {
            //if(!string.IsNullOrEmpty(user.Password))
            //{
                var hashedPassword = Helpers.Helper.GenerateSHA256String(user.Password);
                user.Password = hashedPassword;
            //}
            
            var response = await PutRequest($"api/Users/{user.UserID}", user);
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                throw new System.Exception("Some error in updating driver");

            System.Console.WriteLine($"Driver update response : {response}");
        }

        public async Task<User> GetUserByUsername(string username)
        {
            User user = await GetRequest<User>($"api/Users/GetUserByUsername/{username}");

            return user;
        }
    }
}
