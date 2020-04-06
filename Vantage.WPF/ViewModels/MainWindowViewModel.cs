using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Vantage.Common.Models;
using Vantage.WPF.Controls.Models;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Messages;

namespace Vantage.WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMessagingService _messagingService;
        private readonly IProductService _productService;
        private readonly DelegateCommand _menuItemClickCommand;

        private string _status = string.Empty;
        private bool _isLoggedIn = false;
        private bool _isMenuItemClickInProgress;

        private UserInfo _loggedInUserInfo;
        private IList<Product> _products;
        private Product _selectedProduct;

        public IList<Product> Products 
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        public Product SelectedProduct 
        {
            get { return _selectedProduct; }
            set { SetProperty(ref _selectedProduct, value); }
        }

        public DelegateCommand MenuItemClickCommand { get { return _menuItemClickCommand; } }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                SetProperty(ref _isLoggedIn, value);
            }
        }

        public UserInfo LoggedInUserInfo 
        {
            get { return _loggedInUserInfo; }
            set { SetProperty(ref _loggedInUserInfo, value); }
        } 

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public MainWindowViewModel(INavigationService navigationService, IMessagingService messagingService, IProductService productService)
        {
            _navigationService = navigationService;
            _messagingService = messagingService;
            _productService = productService;
            _menuItemClickCommand = new DelegateCommand(MenuItemClicked, CanMenuItemClicked);
        }

        public void InitializeNavigationService(Frame frame)
        {
            _navigationService.Initialize(frame);
            _navigationService.NavigateTo(Enums.PageKey.Login);
        }

        public async Task GetAllProductsAsync()
        {
            Products = await _productService.GetAllProducts();
        }

        private void MenuItemClicked(object parameter)
        {
            _isMenuItemClickInProgress = true;
            string commandParameter = parameter as string;
            switch (commandParameter.ToLower())
            {
                case "exit":
                    var messageResult = MessageBox.Show("Are you sure you want to exit app?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageResult == MessageBoxResult.Yes)
                        _messagingService.Send<ExitAppMessage>(new ExitAppMessage());
                    break;
                case "login":
                    _navigationService.NavigateTo(Enums.PageKey.Login);
                    break;
                case "logout":
                    Console.WriteLine("Logout button pressed...");
                    break;
                case "training":
                    _navigationService.NavigateTo(Enums.PageKey.TrainingReport);
                    break;
                case "admin":
                    _navigationService.NavigateTo(Enums.PageKey.Admin);
                    break;
                default:
                    break;
            }
            _isMenuItemClickInProgress = false;
        }

        private bool CanMenuItemClicked(object parameter)
        {
            return !_isMenuItemClickInProgress;
        }
    }
}
