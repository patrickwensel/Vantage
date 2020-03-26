using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vantage.WPF
{
    public class Config
    {
        public static int MinimumPassScore = 70;

        public static string GetAPIBaseUrl(IConfiguration configuration)
        {
            return configuration.GetSection("ApiConfig").GetSection("BaseUrl").Value;
        }
    }
}
