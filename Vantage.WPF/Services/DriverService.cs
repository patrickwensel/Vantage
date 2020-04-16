using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vantage.Common.Models;
using Vantage.Common.Utility;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.Services
{
    public class DriverService : BaseAPIService, IDriverService
    {
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;

        public DriverService(IConfiguration iConfig)
        {
            _configuration = iConfig;
            _apiBaseUrl = Config.GetAPIBaseUrl(_configuration);
            SetBaseUrlAndTimeout(_apiBaseUrl);
        }

        public async Task<IList<Driver>> GetAllDrivers()
        {
            IList<Driver> drivers = new List<Driver>();

            drivers = await GetRequest<IList<Driver>>("api/Drivers");

            return drivers;
        }

        public async Task UpdateDriver(Driver driver)
        {
            var response = await PutRequest($"api/Drivers/{driver.DriverID}", driver);
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                throw new System.Exception("Some error in updating driver");

            System.Console.WriteLine($"Driver update response : {response}");
        }

        public async Task<Driver> AddNewDriver(Driver driver)
        {
            var addedDriver = await PostRequest<Driver>("api/Drivers", driver);
            return addedDriver;
        }

        public async Task<Driver> DeleteDriver(int driverId)
        {
            var deletedDriver = await DeleteRequest<Driver>($"api/Drivers/{driverId}");
            return deletedDriver;
        }
    }
}
