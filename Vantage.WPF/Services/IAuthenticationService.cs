using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Vantage.WPF.Services
{
    public interface IAuthenticationService
    {
        Task<UserReturnObject> AuthenticateUser(string username, string password);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private class InternalUserData
        {
            public InternalUserData(string username, string email, string hashedPassword, string[] roles)
            {
                Username = username;
                Email = email;
                HashedPassword = hashedPassword;
                Roles = roles;
            }
            public string Username
            {
                get;
                private set;
            }

            public string Email
            {
                get;
                private set;
            }

            public string HashedPassword
            {
                get;
                private set;
            }

            public string[] Roles
            {
                get;
                private set;
            }
        }

        private static readonly List<InternalUserData> _users = new List<InternalUserData>()
        {
            new InternalUserData("Mark", "mark@company.com",
            "MB5PYIsbI2YzCUe34Q5ZU2VferIoI4Ttd+ydolWV0OE=", new string[] { "Administrators" }),
            new InternalUserData("John", "john@company.com",
            "hMaLizwzOQ5LeOnMuj+C6W75Zl5CXXYbwDSHWW9ZOXc=", new string[] { })
        };

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

        [HttpPost]
        public async Task<UserReturnObject> Authenticate(UserAuthentication userAuthentication)
        {
            UserReturnObject userReturnObject = new UserReturnObject();
            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Add("Key", "Secret@123");
                StringContent content = new StringContent(JsonConvert.SerializeObject(userAuthentication), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{Config.BaseUrl}/api/users/authenticate", content))
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

    //public class User
    //{
    //    public User(string username, string email, string[] roles)
    //    {
    //        Username = username;
    //        Email = email;
    //        Roles = roles;
    //    }
    //    public string Username
    //    {
    //        get;
    //        set;
    //    }

    //    public string Email
    //    {
    //        get;
    //        set;
    //    }

    //    public string[] Roles
    //    {
    //        get;
    //        set;
    //    }
    //}

    public class UserAuthentication
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserReturnObject
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; }

    }
}
