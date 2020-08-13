using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Vantage.Common.Models;
using Vantage.Common.Utility;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.Services
{
    public class DatabaseService : BaseAPIService, IDatabaseService
    {
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;

        public DatabaseService(IConfiguration iConfig)
        {
            _configuration = iConfig;
            _apiBaseUrl = Config.GetAPIBaseUrl(_configuration);
            SetBaseUrlAndTimeout(_apiBaseUrl);
        }

        public async Task<ApiResponse> BackupDatabase(string filePath)
        {
            string backupRequestUrl = @"api/DatabaseUtility/BackUpDatabase/" + Uri.EscapeDataString(filePath.Trim());
            ApiResponse apiResponse = await GetRequest<ApiResponse>(backupRequestUrl);
            return apiResponse;
        }

        public async Task<ApiResponse> RestoreDatabase(string filePath)
        {
            string restoreRequestUrl = @"api/DatabaseUtility/RestoreDatabase/" + Uri.EscapeDataString(filePath.Trim());
            ApiResponse apiResponse = await GetRequest<ApiResponse>(restoreRequestUrl);
            return apiResponse;
        }
    }
}
