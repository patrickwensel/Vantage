using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Vantage.Common.Models;
using Vantage.WPF.Controls.Models;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Models;

namespace Vantage.WPF.ViewModels
{
    public class TrainingReportViewModel : BaseViewModel, IViewModel
    {
        private readonly IGroupService _groupService;
        private readonly IDriverService _driverService;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly Group AllGroupForComboBox = new Group() { GroupID = -1, Name = "All" };
        private readonly ICommand _groupSelectedCommand;
        private readonly ICommand _manageCommand;
        private readonly ICommand _systemCommand;
        private readonly ICommand _selectAllCheckedChangedCommand;
        private readonly ICommand _driverSelectCheckedChangedCommand;
        private readonly ICommand _productSelectedCommand;

        private int _fetchedDriversCount;
        private Group _selectedGroup;
        private UserInfo _loggedInUserInfo;
        private IList<Group> _groups;
        private IList<SelectableDriver> _drivers;
        private IList<TabItem> _tabItems;
        private IList<Product> _products;
        private Product _selectedProduct;
        private bool? _isAllSelected = false;

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

        public IList<SelectableDriver> Drivers
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

        public bool? IsAllSelected
        {
            get { return _isAllSelected; }
            set { SetProperty(ref _isAllSelected, value); }
        }

        public ICommand GroupSelectedCommand { get { return _groupSelectedCommand; } }

        public ICommand ProductSelectedCommand { get { return _productSelectedCommand; } }

        public ICommand SelectAllCheckedChangedCommand { get { return _selectAllCheckedChangedCommand; } }

        public ICommand DriverSelectCheckedChangedCommand { get { return _driverSelectCheckedChangedCommand; } }

        public TrainingReportViewModel(IGroupService groupService, IDriverService driverService, MainWindowViewModel mainWindowViewModel)
        {
            _groupService = groupService;
            _driverService = driverService;
            _mainWindowViewModel = mainWindowViewModel;
            LoggedInUserInfo = mainWindowViewModel.LoggedInUserInfo;
            _selectAllCheckedChangedCommand = new DelegateCommand(OnSelectAllCheckedChanged);
            _driverSelectCheckedChangedCommand = new DelegateCommand(OnDriverSelectCheckedChanged);
            _groupSelectedCommand = new DelegateCommand(OnGroupSelected);
            _productSelectedCommand = new DelegateCommand(OnProductSelected);
            _manageCommand = new DelegateCommand(OnManageClicked);
            _systemCommand = new DelegateCommand(OnSystemClicked);
            TabItems = new List<TabItem>()
            {
                new TabItem() { Icon = "", Text = "Training Reports", IsSelected = true, ClickCommand = null },
                new TabItem() { Icon = "", Text = "Manage Drivers", IsSelected = false, ClickCommand = _manageCommand },
                new TabItem() { Icon = "", Text = "System", IsSelected = false, ClickCommand = _systemCommand },
            };

            Groups = new List<Group>() { AllGroupForComboBox };
        }

        public async Task OnInitializedAsync()
        {
            Products = _mainWindowViewModel.Products;
            SelectedProduct = _mainWindowViewModel.SelectedProduct;

            await FetchGroupsAsync();
            SelectedGroup = Groups[0];

            //SetGroupsAsPerTheSelectedProduct();            
        }

        private async Task FetchDriversAsync()
        {
            var drivers = await _driverService.GetAllDrivers();

            Drivers = GetSelectableDrivers(drivers);
            FetchedDriversCount = Drivers != null ? Drivers.Count : 0;
        }

        private async Task FetchGroupsAsync()
        {
            var groups = await _groupService.GetGroups();
            if (groups == null)
                return;

            Groups = groups.Where(x => x.ProductID == SelectedProduct.ProductID).ToList();
            Groups.Insert(0, AllGroupForComboBox);
            Console.WriteLine($"Groups : {Groups}");
        }

        private void SetGroupsAsPerTheSelectedProduct()
        {
            Groups = SelectedProduct.Groups;
            Groups.Insert(0, AllGroupForComboBox);
            OnPropertyChanged(nameof(Groups));
        }

        private async Task FetchDriversByGroupId(int groupId)
        {
            var group = await _groupService.GetGroup(groupId);

            if (group == null)
                return;

            Drivers = GetSelectableDrivers(group.Drivers);

            foreach (SelectableDriver driver in Drivers)
            {
                driver.Group = new Group()
                {
                    GroupID = group.GroupID,
                    Name = group.Name
                };
            }

            FetchedDriversCount = Drivers != null ? Drivers.Count : 0;
        }

        private void OnSelectAllCheckedChanged(object parameter)
        {
            if (IsAllSelected == null || Drivers == null)
                return;

            Console.WriteLine($"SelectAll Checked : {IsAllSelected.GetValueOrDefault(false)}");
            Drivers = Drivers.Select(x => { x.IsSelected = IsAllSelected.GetValueOrDefault(false); return x; }).ToList();
        }

        private void OnDriverSelectCheckedChanged(object parameter)
        {
            SelectableDriver selectableDriver = parameter as SelectableDriver;
            selectableDriver.IsSelected = !selectableDriver.IsSelected;

            if (Drivers.All(x => x.IsSelected))
                IsAllSelected = true;
            else if (Drivers.Count(x => x.IsSelected) > 0)
                IsAllSelected = null;
            else
                IsAllSelected = false;
        }

        private async void OnGroupSelected(object parameter)
        {
            if (SelectedGroup == null)
                return;

            Console.WriteLine($"Selected Group : {SelectedGroup.GroupID}");
            if (SelectedGroup.GroupID == -1)
                await FetchDriversAsync();
            else
                await FetchDriversByGroupId(SelectedGroup.GroupID);
        }

        private async void OnProductSelected(object parameter)
        {
            if (SelectedProduct == null)
                return;

            //SetGroupsAsPerTheSelectedProduct();
            await FetchGroupsAsync();
        }

        private void OnManageClicked(object parameter)
        {
            Console.WriteLine($"Manage Clicked : {parameter}");
        }

        private void OnSystemClicked(object parameter)
        {
            Console.WriteLine($"System Clicked : {parameter}");
        }

        private IList<SelectableDriver> GetSelectableDrivers(IList<Driver> drivers)
        {
            IList<SelectableDriver> selectableDrivers = new List<SelectableDriver>();
            foreach (Driver driver in drivers)
            {
                selectableDrivers.Add(new SelectableDriver()
                {
                    DriverID = driver.DriverID,
                    FirstName = driver.FirstName,
                    LastName = driver.LastName,
                    UserName = driver.UserName,
                    Pin = driver.Pin,
                    IsActive = driver.IsActive,
                    GroupID = driver.GroupID,
                    Group = driver.Group,
                    Attempts = driver.Attempts
                });
            }

            return selectableDrivers;
        }
    }
}
