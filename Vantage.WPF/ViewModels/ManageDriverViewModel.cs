using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Vantage.Common.Models;
using Vantage.WPF.Controls.Models;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.ViewModels
{
    public class ManageDriverViewModel : BaseViewModel
    {
        private readonly IGroupService _groupService;
        private readonly IDriverService _driverService;
        private readonly INavigationService _navigationService;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly ICommand _trainingCommand;
        private readonly ICommand _systemCommand;
        private readonly ICommand _productSelectedCommand;

        private UserInfo _loggedInUserInfo;
        private IList<TabItem> _tabItems;
        private IList<Product> _products;
        private Product _selectedProduct;
        private IList<Driver> _drivers;

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

        public IList<Driver> Drivers
        {
            get { return _drivers; }
            set { SetProperty(ref _drivers, value); }
        }

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                SetProperty(ref _selectedProduct, value);
                _mainWindowViewModel.SelectedProduct = value;
            }
        }

        public ICommand ProductSelectedCommand { get { return _productSelectedCommand; } }

        public IList<TabItem> TabItems
        {
            get { return _tabItems; }
            set { SetProperty(ref _tabItems, value); }
        }

        public ManageDriverViewModel(IGroupService groupService, IDriverService driverService, INavigationService navigationService, MainWindowViewModel mainWindowViewModel)
        {
            _groupService = groupService;
            _driverService = driverService;
            _navigationService = navigationService;
            _mainWindowViewModel = mainWindowViewModel;
            LoggedInUserInfo = mainWindowViewModel.LoggedInUserInfo;
            _productSelectedCommand = new DelegateCommand(OnProductSelected);            
            _trainingCommand = new DelegateCommand(OnTrainingClicked);
            _systemCommand = new DelegateCommand(OnSystemClicked);
            TabItems = new List<TabItem>()
            {
                new TabItem() { Icon = "", Text = "Training Reports", IsSelected = false, ClickCommand = _trainingCommand },
                new TabItem() { Icon = "", Text = "Manage Drivers", IsSelected = true, ClickCommand = null },
                new TabItem() { Icon = "", Text = "System", IsSelected = false, ClickCommand = _systemCommand },
            };
        }

        public async Task OnInitializedAsync()
        {
            Products = _mainWindowViewModel.Products;
            SelectedProduct = _mainWindowViewModel.SelectedProduct;            
        }

        private async void OnProductSelected(object parameter)
        {
            if (SelectedProduct == null)
                return;            
        }

        private void OnTrainingClicked(object parameter)
        {
            Console.WriteLine($"Training Clicked : {parameter}");
            _navigationService.NavigateTo(Enums.PageKey.TrainingReport);
        }

        private void OnSystemClicked(object parameter)
        {
            Console.WriteLine($"System Clicked : {parameter}");
            _navigationService.NavigateTo(Enums.PageKey.System);
        }
    }
}
