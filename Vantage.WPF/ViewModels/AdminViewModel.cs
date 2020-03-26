using System;
using System.Collections.ObjectModel;
using Vantage.WPF.Interfaces;
using Vantage.Common.Models;
using Vantage.WPF.Services;

namespace Vantage.WPF.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private readonly IProductService _productService;

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get
            {
                return products;
            }

            set
            {
                SetProperty(ref products, value);
            }
        }

        public AdminViewModel(IProductService productService)
        {
            _productService = productService;
            LoadData();
        }

        //public AdminViewModel()
        //{
        //    LoadData();
        //}

        private async void LoadData()
        {
            Products = await _productService.GetAllProducts();
            Console.WriteLine($"Products : {Products}");
        }
    }
}
