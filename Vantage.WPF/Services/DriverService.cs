using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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

        public async Task<IList<Driver>> GetDriversByGroupID(int Id)
        {
            IList<Driver> drivers = new List<Driver>();

            drivers = await GetRequest<IList<Driver>>($"api/Drivers/GetDriversByGroupId/{Id}");

            return drivers;
        }
    }
}
