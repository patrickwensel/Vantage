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
using Vantage.WPF.Models;

namespace Vantage.WPF.Services
{
    public interface IProductService
    {
        Task<ObservableCollection<Product>> GetAllProducts();
    }
    public class ProductService : IProductService
    {
        [HttpGet]
        public async Task<ObservableCollection<Product>> GetAllProducts()
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Config.BaseUrl}/api/products"))
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
