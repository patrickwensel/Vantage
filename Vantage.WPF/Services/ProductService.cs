using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Vantage.WPF.Interfaces;
using Vantage.Common.Models;
using Microsoft.Extensions.Configuration;

namespace Vantage.WPF.Services
{
    public class ProductService : IProductService
    {
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;

        public ProductService(IConfiguration iConfig)
        {
            _configuration = iConfig;
            _apiBaseUrl = _configuration.GetSection("ApiConfig").GetSection("BaseUrl").Value;
        }

        [HttpGet]
        public async Task<ObservableCollection<Product>> GetAllProducts()
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_apiBaseUrl}/api/products"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    try
                    {
                        products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(apiResponse);
                    }
                    catch (Exception ex)
                    {
                        //ViewBag.Result = apiResponse;
                        //return View();
                    }
                }
            }

            return products;
        }
    }
}
