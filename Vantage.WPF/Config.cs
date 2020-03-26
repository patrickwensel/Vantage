using Microsoft.Extensions.Configuration;

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
