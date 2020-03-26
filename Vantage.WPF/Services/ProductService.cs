using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Vantage.Common.Models;
using Vantage.Common.Utility;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.Services
{
    public class ProductService : BaseAPIService, IProductService
    {
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;

        public ProductService(IConfiguration iConfig)
        {
            _configuration = iConfig;
            _apiBaseUrl = Config.GetAPIBaseUrl(_configuration);
            SetBaseUrlAndTimeout(_apiBaseUrl);
        }

        [HttpGet]
        public async Task<ObservableCollection<Product>> GetAllProducts()
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();

            products = await GetRequest<ObservableCollection<Product>>("/api/products");

            return products;
        }
    }
}
