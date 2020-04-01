﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Vantage.Common.Models;
using Vantage.WPF.Controls.Models;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly IProductService _productService;

        private UserInfo _loggedInUserInfo;
        private IList<Product> _products;
        private Product _selectedProduct;

        private ICommand _reportCommand;
        private ICommand _manageCommand;
        private ICommand _systemCommand;

        public UserInfo LoggedInUserInfo 
        {
            get { return _loggedInUserInfo; }
            set { SetProperty(ref _loggedInUserInfo, value); }
        }

        public IList<Product> Products 
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        public ICommand ReportCommand { get { return _reportCommand; } }

        public ICommand ManageCommand { get { return _manageCommand; } }

        public ICommand SystemCommand { get { return _systemCommand; } }

        public Product SelectedProduct 
        {
            get { return _selectedProduct; }
            set { SetProperty(ref _selectedProduct, value); }
        }

        public DashboardViewModel(INavigationService navigationService, IProductService productService, MainWindowViewModel mainWindowViewModel)
        {
            _navigationService = navigationService;
            _productService = productService;
            _mainWindowViewModel = mainWindowViewModel;

            _reportCommand = new DelegateCommand(NavigateOnReportScreen);
            _manageCommand = new DelegateCommand(NavigateOnManageScreen);
            _systemCommand = new DelegateCommand(NavigateOnSystemScreen);
        }

        public async Task OnInitializedAsync()
        {
            LoggedInUserInfo = _mainWindowViewModel.LoggedInUserInfo;
            Products = await _productService.GetAllProducts();
        }

        private void NavigateOnReportScreen(object parameter)
        {
            Console.WriteLine($"Selected Product Is : {SelectedProduct}");
            _navigationService.NavigateTo(Enums.PageKey.TrainingReport);
        }

        private void NavigateOnManageScreen(object parameter)
        {
            Console.WriteLine($"Selected Product Is : {SelectedProduct}");
        }

        private void NavigateOnSystemScreen(object parameter)
        {
            Console.WriteLine($"Selected Product Is : {SelectedProduct}");
        }
    }
}
