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
    public class GroupService : BaseAPIService, IGroupService
    {
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;

        public GroupService(IConfiguration iConfig)
        {
            _configuration = iConfig;
            _apiBaseUrl = Config.GetAPIBaseUrl(_configuration);
            SetBaseUrlAndTimeout(_apiBaseUrl);
        }

        public async Task<IList<Group>> GetGroups()
        {
            return await GetRequest<IList<Group>>("/api/Groups");
        }
    }
}
