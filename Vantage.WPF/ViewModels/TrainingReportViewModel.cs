using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly ICommand _showActiveDriverStateChangedCommand;

        private int _fetchedDriversCount;
        private Group _selectedGroup;
        private UserInfo _loggedInUserInfo;
        private ObservableCollection<Group> _groups;
        private IList<SelectableDriver> _allDrivers;
        private IList<SelectableDriver> _drivers;
        private IList<TabItem> _tabItems;
        private IList<Product> _products;
        private Product _selectedProduct;
        private bool? _isAllSelected = false;
        private bool _showOnlyActiveDrivers = true;

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

        public ObservableCollection<Group> Groups
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

        public bool ShowOnlyActiveDrivers
        {
            get { return _showOnlyActiveDrivers; }
            set { SetProperty(ref _showOnlyActiveDrivers, value); }
        }

        public ICommand GroupSelectedCommand { get { return _groupSelectedCommand; } }

        public ICommand ProductSelectedCommand { get { return _productSelectedCommand; } }

        public ICommand SelectAllCheckedChangedCommand { get { return _selectAllCheckedChangedCommand; } }

        public ICommand DriverSelectCheckedChangedCommand { get { return _driverSelectCheckedChangedCommand; } }

        public ICommand ShowActiveDriverStateChangedCommand { get { return _showActiveDriverStateChangedCommand; } }

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
            _showActiveDriverStateChangedCommand = new DelegateCommand(OnShowActiveDriverStateChanged);
            _manageCommand = new DelegateCommand(OnManageClicked);
            _systemCommand = new DelegateCommand(OnSystemClicked);
            TabItems = new List<TabItem>()
            {
                new TabItem() { Icon = "", Text = "Training Reports", IsSelected = true, ClickCommand = null },
                new TabItem() { Icon = "", Text = "Manage Drivers", IsSelected = false, ClickCommand = _manageCommand },
                new TabItem() { Icon = "", Text = "System", IsSelected = false, ClickCommand = _systemCommand },
            };

            Groups = new ObservableCollection<Group>() { AllGroupForComboBox };
        }

        public async Task OnInitializedAsync()
        {
            Products = _mainWindowViewModel.Products;
            SelectedProduct = _mainWindowViewModel.SelectedProduct;

            await FetchGroupsAsync();
            SelectedGroup = Groups[0];

            //SetGroupsAsPerTheSelectedProduct();            
        }

        private async Task FetchGroupsAsync()
        {
            ClearDriverList();
            Groups.Clear();
            var groups = await _groupService.GetGroups();

            if (groups == null)
                return;

            UpdateGroupList(groups.Where(x => x.ProductID == SelectedProduct.ProductID).ToList());
            Console.WriteLine($"Groups : {Groups}");
        }

        private void UpdateGroupList(IList<Group> groups)
        {
            Groups.Clear();
            Groups.Add(AllGroupForComboBox);

            if (groups == null)
            {
                SelectedGroup = Groups[0];
                return;
            }

            foreach (Group group in groups)
            {
                Groups.Add(group);
            }

            SelectedGroup = Groups[0];
        }

        private void FetchDriversFromAllTheGroups()
        {
            ClearDriverList();
            if (Groups == null)
                return;

            var listOfDrivers = Groups.Select(x => x.Drivers).ToList();

            List<Driver> driversList = new List<Driver>();
            foreach (IList<Driver> drivers in listOfDrivers)
            {
                if (drivers == null)
                    continue;

                driversList.AddRange(drivers);
            }

            _allDrivers = GetSelectableDrivers(driversList);
            GetDriversBasedOnActiveStatus(ShowOnlyActiveDrivers);
        }

        private async Task FetchDriversByGroupId(int groupId)
        {
            ClearDriverList();
            var group = await _groupService.GetGroup(groupId);

            _allDrivers = GetSelectableDrivers(group.Drivers);

            if (_allDrivers != null)
            {
                foreach (SelectableDriver driver in _allDrivers)
                {
                    driver.Group = new Group()
                    {
                        GroupID = group.GroupID,
                        Name = group.Name
                    };
                }
            }

            GetDriversBasedOnActiveStatus(ShowOnlyActiveDrivers);
        }        

        private void SetGroupsAsPerTheSelectedProduct()
        {
            UpdateGroupList(SelectedProduct.Groups);
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

        private void OnShowActiveDriverStateChanged(object parameter)
        {
            Console.WriteLine($"ShowActiveDriver Status Changed : {ShowOnlyActiveDrivers}");
            Drivers = GetDriversBasedOnActiveStatus(ShowOnlyActiveDrivers);
        }

        private async void OnGroupSelected(object parameter)
        {
            if (SelectedGroup == null)
                return;

            Console.WriteLine($"Selected Group : {SelectedGroup.GroupID}");
            if (SelectedGroup.GroupID == -1)
                FetchDriversFromAllTheGroups();
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

        private IList<SelectableDriver> GetDriversBasedOnActiveStatus(bool isActive = true)
        {            
            if (_allDrivers == null)
                return null;

            Drivers = isActive ? _allDrivers.Where(x => x.IsActive).ToList() : _allDrivers;
            FetchedDriversCount = Drivers != null ? Drivers.Count : 0;

            return Drivers;
        }

        private void ClearDriverList()
        {
            if (Drivers != null)
                Drivers.Clear();

            Drivers = null;
            FetchedDriversCount = 0;
        }
    }
}
