using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Vantage.Common.Models;
using Vantage.WPF.Controls.Models;
using Vantage.WPF.Helpers;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Models;

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
        private readonly ICommand _addNewDriverCommand;
        private readonly ICommand _editDriverCommand;
        private readonly ICommand _deleteDriverCommand;
        private readonly ICommand _driversGroupUpdatedCommand;
        private readonly ICommand _editCommand;
        private readonly ICommand _addCommand;
        private readonly ICommand _closePopupCommand;

        private UserInfo _loggedInUserInfo;
        private IList<TabItem> _tabItems;
        private IList<Product> _products;
        private Product _selectedProduct;
        private ObservableCollection<Group> _groups;
        private IList<Driver> _drivers;
        private Driver _editingDriver;
        private int _fetchedDriversCount;

        private bool _isEditingDriver = false;
        private bool _isAddingDriver = false;

        private string _errorMessage;
        private string _firstName;
        private string _lastName;
        private string _username;
        private string _pin;
        private Group _addEditSelectedGroup;
        private bool _isActive = true;
        private bool _isErrorInFirstName;
        private bool _isErrorInLastName;
        private bool _isErrorInUsername;
        private bool _isErrorInPin;
        private bool _isErrorInGroup;

        public EventHandler OnErrorOccurred;

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

        public ObservableCollection<Group> Groups
        {
            get { return _groups; }
            set { SetProperty(ref _groups, value); }
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

        public Driver EditingDriver
        {
            get { return _editingDriver; }
            set { SetProperty(ref _editingDriver, value); }
        }

        public int FetchedDriversCount
        {
            get { return _fetchedDriversCount; }
            private set { SetProperty(ref _fetchedDriversCount, value); }
        }

        public bool IsEditingDriver
        {
            get { return _isEditingDriver; }
            set { SetProperty(ref _isEditingDriver, value); }
        }

        public bool IsAddingDriver
        {
            get { return _isAddingDriver; }
            set { SetProperty(ref _isAddingDriver, value); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string Pin
        {
            get { return _pin; }
            set { SetProperty(ref _pin, value); }
        }

        public Group AddEditSelectedGroup
        {
            get { return _addEditSelectedGroup; }
            set { SetProperty(ref _addEditSelectedGroup, value); }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value); }
        }

        public bool IsErrorInFirstName
        {
            get { return _isErrorInFirstName; }
            set { SetProperty(ref _isErrorInFirstName, value); }
        }

        public bool IsErrorInLastName
        {
            get { return _isErrorInLastName; }
            set { SetProperty(ref _isErrorInLastName, value); }
        }

        public bool IsErrorInUsername
        {
            get { return _isErrorInUsername; }
            set { SetProperty(ref _isErrorInUsername, value); }
        }

        public bool IsErrorInPin
        {
            get { return _isErrorInPin; }
            set { SetProperty(ref _isErrorInPin, value); }
        }

        public bool IsErrorInGroup
        {
            get { return _isErrorInGroup; }
            set { SetProperty(ref _isErrorInGroup, value); }
        }

        public ICommand ProductSelectedCommand { get { return _productSelectedCommand; } }

        public ICommand AddNewDriverCommand { get { return _addNewDriverCommand; } }

        public ICommand EditDriverCommand { get { return _editDriverCommand; } }

        public ICommand DeleteDriverCommand { get { return _deleteDriverCommand; } }

        public ICommand DriversGroupUpdatedCommand { get { return _driversGroupUpdatedCommand; } }

        public ICommand EditCommand { get { return _editCommand; } }

        public ICommand AddCommand { get { return _addCommand; } }

        public ICommand ClosePopupCommand { get { return _closePopupCommand; } }

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
            _driversGroupUpdatedCommand = new DelegateCommand(OnDriversGroupUpdated);
            _addNewDriverCommand = new DelegateCommand(OpenAddDriverPopup);
            _editDriverCommand = new DelegateCommand(OpenEditDriverPopup);
            _deleteDriverCommand = new DelegateCommand(OnDeleteDriver);
            _editCommand = new DelegateCommand(EditDriver);
            _addCommand = new DelegateCommand(AddDriver);
            _closePopupCommand = new DelegateCommand(ClosePopup);

            TabItems = new List<TabItem>()
            {
                new TabItem() { Icon = "", Text = "Training Reports", IsSelected = false, ClickCommand = _trainingCommand },
                new TabItem() { Icon = "", Text = "Manage Drivers", IsSelected = true, ClickCommand = null },
                new TabItem() { Icon = "", Text = "System", IsSelected = false, ClickCommand = _systemCommand },
            };

            Groups = new ObservableCollection<Group>();
        }

        public async Task OnInitializedAsync()
        {
            Products = _mainWindowViewModel.Products;
            SelectedProduct = _mainWindowViewModel.SelectedProduct;
            if (SelectedProduct == null)
                return;

            await FetchGroupsAsync();
            //FetchDriversFromAllTheGroups();
            await FetchAllDriversAsync();
        }

        private async Task FetchGroupsAsync(bool shouldClearDriverList = true)
        {
            App.SetCursorToWait();
            if (shouldClearDriverList)
                ClearDriverList();

            Groups.Clear();
            var groups = await _groupService.GetGroups();

            UpdateGroupList(groups != null ? groups.Where(x => x.ProductID == SelectedProduct.ProductID).ToList() : null);
            App.SetCursorToArrow();
            Console.WriteLine($"Groups : {Groups}");
        }

        private void UpdateGroupList(IList<Group> groups)
        {
            Groups.Clear();
            if (groups == null)
                return;

            foreach (Group group in groups)
            {
                Groups.Add(group);
            }
        }

        private void FetchDriversFromAllTheGroups()
        {
            ClearDriverList();
            if (Groups == null)
                return;

            List<Driver> driversList = new List<Driver>();

            foreach (Group group in Groups)
            {
                if (group == null || group.Drivers == null)
                    continue;

                foreach (Driver driver in group.Drivers)
                {
                    if (driver == null)
                        continue;

                    driver.Group = group;
                    driversList.Add(driver);
                }
            }

            Drivers = driversList;
            Console.WriteLine($"Drivers : {Drivers}");
        }

        private async Task FetchAllDriversAsync()
        {
            ClearDriverList();
            IList<Driver> driversList;

            driversList = await _driverService.GetAllDrivers();
            foreach (Driver driver in driversList)
            {
                if (driver.GroupID == null)
                    continue;

                var group = Groups.FirstOrDefault(x => x.GroupID == driver.GroupID);
                driver.Group = group;
            }
            Drivers = driversList != null ? driversList : new List<Driver>();
            Console.WriteLine($"Drivers : {Drivers}");
        }

        private void ClearDriverList()
        {
            if (Drivers != null)
                Drivers.Clear();

            Drivers = null;
            FetchedDriversCount = 0;
        }

        private async void OnProductSelected(object parameter)
        {
            if (SelectedProduct == null)
                return;

            await FetchGroupsAsync();
            await FetchAllDriversAsync();
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

        private async void OnDriversGroupUpdated(object parameter)
        {
            Console.WriteLine($"Group updated for Driver : {parameter}");
            UpdateDriversGroup updateDriverGroup = parameter as UpdateDriversGroup;
            if (updateDriverGroup.Group == null || updateDriverGroup.Driver.GroupID == updateDriverGroup.Group.GroupID)
                return;

            Driver driver = new Driver()
            {
                LastName = updateDriverGroup.Driver.LastName,
                FirstName = updateDriverGroup.Driver.FirstName,
                UserName = updateDriverGroup.Driver.UserName,
                DriverID = updateDriverGroup.Driver.DriverID,
                Pin = updateDriverGroup.Driver.Pin,
                IsActive = updateDriverGroup.Driver.IsActive,
                GroupID = updateDriverGroup.Group.GroupID,
                ProductID = updateDriverGroup.Driver.ProductID,
            };

            await UpdateDriverAsync(driver);
        }

        private async Task UpdateDriverAsync(Driver driver)
        {
            App.SetCursorToWait();
            await _driverService.UpdateDriver(driver);
            App.SetCursorToArrow();
        }

        private void OpenAddDriverPopup(object parameter)
        {
            Console.WriteLine("New Driver added...");
            IsEditingDriver = false;
            IsAddingDriver = true;
        }

        private void OpenEditDriverPopup(object parameter)
        {
            Console.WriteLine($"Edit Driver : {parameter}");
            IsAddingDriver = false;
            IsEditingDriver = true;
            EditingDriver = parameter as Driver;

            FirstName = EditingDriver.FirstName;
            LastName = EditingDriver.LastName;
            Username = EditingDriver.UserName;
            Pin = EditingDriver.Pin;
            AddEditSelectedGroup = Groups.First(x => x.GroupID == EditingDriver.GroupID);
            IsActive = EditingDriver.IsActive;
        }

        private async void EditDriver(object parameter)
        {
            Console.WriteLine("Editing driver");
            if (!ValidateDriverInfo())
                return;

            Driver driver = new Driver()
            {
                DriverID = this.EditingDriver.DriverID,
                FirstName = this.FirstName,
                LastName = this.LastName,
                UserName = this.Username,
                Pin = this.Pin,
                IsActive = this.IsActive,
                GroupID = this.AddEditSelectedGroup != null ? this.AddEditSelectedGroup.GroupID : default(int?),
                ProductID = this.SelectedProduct.ProductID,
            };

            await UpdateDriverAsync(driver);
            await FetchGroupsAsync(false);
            await FetchAllDriversAsync();
            ClosePopup(parameter);
        }

        private async void AddDriver(object parameter)
        {
            Console.WriteLine("Adding driver");           
            if (!ValidateDriverInfo())
                return;

            Driver existingDriver = await _driverService.GetDriverByUsername(this.Username);

            if (existingDriver != null && existingDriver.DriverID != 0)
            {
                IsErrorInUsername = true;
                ErrorMessage = "Username already exists, Please try another.";
                OnErrorOccurred?.Invoke(this, new EventArgs());
                return;
            }

            Driver driver = new Driver()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                UserName = this.Username,
                Pin = this.Pin,
                IsActive = this.IsActive,
                GroupID = this.AddEditSelectedGroup != null ? this.AddEditSelectedGroup.GroupID : default(int?),
                ProductID = this.SelectedProduct.ProductID,               
            };

            await _driverService.AddNewDriver(driver);
            await FetchGroupsAsync(false);
            await FetchAllDriversAsync();

            Console.WriteLine($"FirstName : {FirstName}, LastName : {LastName}, Pin : {Pin}, Group : {AddEditSelectedGroup?.Name}, IsActive : {IsActive}");
            ClosePopup(parameter);
        }

        private void ClosePopup(object parameter)
        {
            IsEditingDriver = false;
            IsAddingDriver = false;
            ClearAddEditDriverProperties();
        }

        private async void OnDeleteDriver(object parameter)
        {
            Driver driver = parameter as Driver;
            var shouldDelete = System.Windows.MessageBox.Show("Are you Sure?", "Delete", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            if (shouldDelete != System.Windows.MessageBoxResult.Yes)
                return;

            App.SetCursorToWait();
            var deletedDriver = await _driverService.DeleteDriver(driver.DriverID);
            Console.WriteLine($"Deleted driver : {deletedDriver}");
            await FetchGroupsAsync(false);
            await FetchAllDriversAsync();
            App.SetCursorToArrow();
        }

        private bool ValidateDriverInfo()
        {
            ErrorMessage = null;
            IsErrorInFirstName = !ValidationHelper.IsValidAlphaString(FirstName);            
            IsErrorInLastName = !ValidationHelper.IsValidAlphaString(LastName);            
            IsErrorInUsername = !ValidationHelper.IsValidAlphanumericString(Username);            
            IsErrorInPin = !ValidationHelper.IsValidDigit(Pin, 4);
            //IsErrorInGroup = AddEditSelectedGroup == null;            
            IsErrorInGroup = false;

            if (IsErrorInFirstName || IsErrorInLastName || IsErrorInUsername || IsErrorInPin || IsErrorInGroup)
            {
                OnErrorOccurred?.Invoke(this, new EventArgs());
            }

            return !(IsErrorInFirstName || IsErrorInLastName || IsErrorInUsername || IsErrorInPin || IsErrorInGroup);
        }

        private void ClearAddEditDriverProperties()
        {
            ErrorMessage = null;
            FirstName = null;
            LastName = null;
            Username = null;
            Pin = null;
            IsActive = true;
            AddEditSelectedGroup = null;

            IsErrorInFirstName = false;
            IsErrorInLastName = false;
            IsErrorInUsername = false;
            IsErrorInPin = false;
            IsErrorInGroup = false;
        }
    }
}
