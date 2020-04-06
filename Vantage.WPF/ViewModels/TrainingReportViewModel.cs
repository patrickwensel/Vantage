using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Vantage.Common.Models;
using Vantage.WPF.Controls.Models;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.ViewModels
{
    public class TrainingReportViewModel : BaseViewModel, IViewModel
    {
        private readonly IGroupService _groupService;
        private readonly IDriverService _driverService;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly ICommand _groupSelectedCommand;
        private readonly ICommand _manageCommand;
        private readonly ICommand _systemCommand;
        private readonly ICommand _productSelectedCommand;

        private int _fetchedDriversCount;
        private Group _selectedGroup;
        private UserInfo _loggedInUserInfo;
        private IList<Group> _groups;
        private IList<Driver> _drivers;
        private IList<TabItem> _tabItems;
        private IList<Product> _products;
        private Product _selectedProduct;

        public int FetchedDriversCount
        {
            get { return _fetchedDriversCount; }
            private set { SetProperty(ref _fetchedDriversCount, value); }
        }

        public UserInfo LoggedInUserInfo
        {
            get { return _loggedInUserInfo; }
            set { SetProperty(ref _loggedInUserInfo, value); }
        }

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set { SetProperty(ref _selectedGroup, value); }
        }

        public IList<Group> Groups
        {
            get { return _groups; }
            set { SetProperty(ref _groups, value); }
        }

        public IList<Driver> Drivers
        {
            get { return _drivers; }
            set { SetProperty(ref _drivers, value); }
        }

        public IList<Product> Products 
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
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

        public IList<TabItem> TabItems 
        {
            get { return _tabItems; }
            set { SetProperty(ref _tabItems, value); }
        }

        public ICommand GroupSelectedCommand { get { return _groupSelectedCommand; } }

        public ICommand ProductSelectedCommand { get { return _productSelectedCommand; } }

        public TrainingReportViewModel(IGroupService groupService, IDriverService driverService, MainWindowViewModel mainWindowViewModel)
        {
            _groupService = groupService;
            _driverService = driverService;
            _mainWindowViewModel = mainWindowViewModel;
            LoggedInUserInfo = mainWindowViewModel.LoggedInUserInfo;
            _groupSelectedCommand = new DelegateCommand(OnGroupSelected);
            _productSelectedCommand = new DelegateCommand(OnProductSelected);
            _manageCommand = new DelegateCommand(OnManageClicked);
            _systemCommand = new DelegateCommand(OnSystemClicked);
            TabItems = new List<TabItem>()
            {
                new TabItem() { Icon = "", Text = "Training Reports", IsSelected = true, ClickCommand = null },
                new TabItem() { Icon = "", Text = "Manage", IsSelected = false, ClickCommand = _manageCommand },
                new TabItem() { Icon = "", Text = "System", IsSelected = false, ClickCommand = _systemCommand },
            };
        }

        public async Task OnInitializedAsync()
        {
            Products = _mainWindowViewModel.Products;
            SelectedProduct = _mainWindowViewModel.SelectedProduct;
            //await FetchGroupsAsync();
            SetGroupsAsPerTheSelectedProduct();
            await FetchDriversAsync();
        }

        private async Task FetchDriversAsync()
        {
            Drivers = await _driverService.GetAllDrivers();

            FetchedDriversCount = Drivers != null ? Drivers.Count : 0;
        }

        private async Task FetchGroupsAsync()
        {            
            var groups = await _groupService.GetGroups();
            Groups = groups.Where(x => x.ProductID == SelectedProduct.ProductID).ToList();
            Console.WriteLine($"Groups : {Groups}");
        }

        private void SetGroupsAsPerTheSelectedProduct()
        {
            Groups = SelectedProduct.Groups;
        }

        private async Task FetchDriversByGroupId(int groupId)
        {
            var group = await _groupService.GetGroup(groupId);

            Drivers = group.Drivers;

            foreach(Driver driver in Drivers)
            {
                driver.Group = new Group()
                {
                    GroupID = group.GroupID,
                    Name = group.Name
                };
            }

            FetchedDriversCount = Drivers != null ? Drivers.Count : 0;
        }

        private async void OnGroupSelected(object parameter)
        {
            if (SelectedGroup == null)
                return;

            Console.WriteLine($"Selected Group : {SelectedGroup.GroupID}");
            await FetchDriversByGroupId(SelectedGroup.GroupID);
        }

        private void OnProductSelected(object parameter)
        {
            if (SelectedProduct == null)
                return;

            SetGroupsAsPerTheSelectedProduct();
            //await FetchGroupsAsync();
        }

        private void OnManageClicked(object parameter)
        {
            Console.WriteLine($"Manage Clicked : {parameter}");
        }

        private void OnSystemClicked(object parameter)
        {
            Console.WriteLine($"System Clicked : {parameter}");
        }
    }
}
