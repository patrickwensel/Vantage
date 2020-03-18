﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Models;
using Vantage.WPF.Services;

namespace Vantage.WPF.ViewModels
{
    public class AdminWindowViewModel : IViewModel, INotifyPropertyChanged
    {
        private readonly IProductService _productService;

        public AdminWindowViewModel(IProductService productService)
        {
            _productService = productService;
        }

        private ObservableCollection<Product> products;

        public ObservableCollection<Product> Products
        {
            get
            {
                return products;
            }
            set
            {
                products = value;
                NotifyPropertyChanged("Products");
            }
        }

        public AdminWindowViewModel()
        {
            LoadData();
        }

        private async void LoadData()
        {
            products = await _productService.GetAllProducts();
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
