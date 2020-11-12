using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        private readonly INavigationService _navigationService;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly Group AllGroupForComboBox = new Group() { GroupID = -1, Name = "All" };
        private readonly ICommand _groupSelectedCommand;
        private readonly ICommand _manageCommand;
        private readonly ICommand _systemCommand;
        private readonly ICommand _selectAllCheckedChangedCommand;
        private readonly ICommand _driverSelectCheckedChangedCommand;
        private readonly ICommand _driversGroupUpdatedCommand;
        private readonly ICommand _productSelectedCommand;
        private readonly ICommand _showActiveDriverStateChangedCommand;
        private readonly ICommand _exportReportCommand;

        private int _fetchedDriversCount;
        private Group _selectedGroup;
        private UserInfo _loggedInUserInfo;
        private ObservableCollection<Group> _groups;
        private ObservableCollection<Group> _onlyGroups;
        private IList<SelectableDriver> _allDrivers;
        private IList<SelectableDriver> _drivers;
        private IList<TabItem> _tabItems;
        private IList<Product> _products;
        private Product _selectedProduct;
        private bool? _isAllSelected = false;
        private bool _showOnlyActiveDrivers = true;
        private bool _isReportTypeDropdownEnabled = false;
        private string _selectedReportType;
        private string _previouslySelectedReportType;
        private string _selectedExportType;
        private string _previouslySelectedExportType;

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

        public ObservableCollection<Group> OnlyGroups
        {
            get { return _onlyGroups; }
            set { SetProperty(ref _onlyGroups, value); }
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

        public bool IsReportTypeDropdownEnabled
        {
            get { return _isReportTypeDropdownEnabled; }
            set { SetProperty(ref _isReportTypeDropdownEnabled, value); }
        }

        public string SelectedReportType
        {
            get { return _selectedReportType; }
            set { SetProperty(ref _selectedReportType, value); }
        }

        public string SelectedExportType
        {
            get { return _selectedExportType; }
            set { SetProperty(ref _selectedExportType, value); }
        }

        public ICommand GroupSelectedCommand { get { return _groupSelectedCommand; } }

        public ICommand ProductSelectedCommand { get { return _productSelectedCommand; } }

        public ICommand SelectAllCheckedChangedCommand { get { return _selectAllCheckedChangedCommand; } }

        public ICommand DriverSelectCheckedChangedCommand { get { return _driverSelectCheckedChangedCommand; } }

        public ICommand DriversGroupUpdatedCommand { get { return _driversGroupUpdatedCommand; } }

        public ICommand ShowActiveDriverStateChangedCommand { get { return _showActiveDriverStateChangedCommand; } }

        public ICommand ExportReportCommand { get { return _exportReportCommand; } }

        public TrainingReportViewModel(IGroupService groupService, IDriverService driverService, INavigationService navigationService, MainWindowViewModel mainWindowViewModel)
        {
            _groupService = groupService;
            _driverService = driverService;
            _navigationService = navigationService;
            _mainWindowViewModel = mainWindowViewModel;
            LoggedInUserInfo = mainWindowViewModel.LoggedInUserInfo;
            _selectAllCheckedChangedCommand = new DelegateCommand(OnSelectAllCheckedChanged);
            _driverSelectCheckedChangedCommand = new DelegateCommand(OnDriverSelectCheckedChanged);
            _driversGroupUpdatedCommand = new DelegateCommand(OnDriversGroupUpdated);
            _groupSelectedCommand = new DelegateCommand(OnGroupSelected);
            _productSelectedCommand = new DelegateCommand(OnProductSelected);
            _showActiveDriverStateChangedCommand = new DelegateCommand(OnShowActiveDriverStateChanged);
            _manageCommand = new DelegateCommand(OnManageClicked);
            _systemCommand = new DelegateCommand(OnSystemClicked);
            _exportReportCommand = new DelegateCommand(ExportReports);
            TabItems = new List<TabItem>()
            {
                new TabItem() { Icon = "", Text = "Training Reports", IsSelected = true, ClickCommand = null },
                new TabItem() { Icon = "", Text = "Manage Drivers", IsSelected = false, ClickCommand = _manageCommand },
                new TabItem() { Icon = "", Text = "System", IsSelected = false, ClickCommand = _systemCommand },
            };

            Groups = new ObservableCollection<Group>() { AllGroupForComboBox };
            OnlyGroups = new ObservableCollection<Group>();
        }

        public async Task OnInitializedAsync()
        {
            Products = _mainWindowViewModel.Products;
            SelectedProduct = _mainWindowViewModel.SelectedProduct;
            if (SelectedProduct == null)
            {
                ClearDriverList();
                Groups.Clear();
                OnlyGroups.Clear();
                return;
            }

            App.SetCursorToWait();
            await FetchGroupsAsync();
            SelectedGroup = Groups[0];

            //SetGroupsAsPerTheSelectedProduct(); 
            App.SetCursorToArrow();
        }

        private async Task FetchGroupsAsync()
        {
            App.SetCursorToWait();
            ClearDriverList();
            Groups.Clear();
            OnlyGroups.Clear();

            var groups = await _groupService.GetGroups();

            UpdateGroupList(groups != null ? groups.Where(x => x.ProductID == SelectedProduct.ProductID).ToList() : null);
            Console.WriteLine($"Groups : {Groups}");
            App.SetCursorToWait();
        }

        private void UpdateGroupList(IList<Group> groups)
        {
            Groups.Clear();
            OnlyGroups.Clear();
            Groups.Add(AllGroupForComboBox);

            if (groups == null)
            {
                SelectedGroup = Groups[0];
                return;
            }

            foreach (Group group in groups)
            {
                Groups.Add(group);
                OnlyGroups.Add(group);
            }

            SelectedGroup = Groups[0];
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

                    driver.Group = new Group() { GroupID = group.GroupID, Name = group.Name };
                    driversList.Add(driver);
                }
            }

            _allDrivers = GetSelectableDrivers(driversList);
            GetDriversBasedOnActiveStatus(ShowOnlyActiveDrivers);
        }

        private async Task FetchAllDriversAsync()
        {
            App.SetCursorToWait();
            ClearDriverList();
            IList<Driver> driversList;

            driversList = await _driverService.GetAllDrivers();
            driversList = driversList.Where(x => x.ProductID == SelectedProduct.ProductID).ToList();
            foreach (Driver driver in driversList)
            {
                if (driver.GroupID == null)
                    continue;

                var group = Groups.FirstOrDefault(x => x.GroupID == driver.GroupID);
                driver.Group = group;
            }

            _allDrivers = GetSelectableDrivers(driversList);
            GetDriversBasedOnActiveStatus(ShowOnlyActiveDrivers);
            Console.WriteLine($"Drivers : {Drivers}");
            App.SetCursorToArrow();
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
            EnableDisableReportTypeDropdown();
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

            EnableDisableReportTypeDropdown();
        }

        private async void OnDriversGroupUpdated(object parameter)
        {
            Console.WriteLine($"Group updated for Driver : {parameter}");
            UpdateDriversGroup updateDriverGroup = parameter as UpdateDriversGroup;
            if (updateDriverGroup == null || updateDriverGroup.Group == null || updateDriverGroup.Driver.GroupID == updateDriverGroup.Group.GroupID)
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
            OnGroupSelected(null);
        }

        private async Task UpdateDriverAsync(Driver driver)
        {
            App.SetCursorToWait();
            await _driverService.UpdateDriver(driver);
            App.SetCursorToArrow();
        }

        private void EnableDisableReportTypeDropdown()
        {
            IsReportTypeDropdownEnabled = Drivers != null ? Drivers.Any(x => x.IsSelected) : false;
            if (!IsReportTypeDropdownEnabled)
            {
                _previouslySelectedReportType = SelectedReportType;
                _previouslySelectedExportType = SelectedExportType;
                SelectedReportType = null;
                SelectedExportType = null;
            }
            else
            {
                SelectedReportType = _previouslySelectedReportType;
                SelectedExportType = _previouslySelectedExportType;
            }
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
                await FetchAllDriversAsync();
            else
                await FetchDriversByGroupId(SelectedGroup.GroupID);
        }

        private async void OnProductSelected(object parameter)
        {
            if (SelectedProduct == null)
                return;

            await FetchGroupsAsync();
            IsAllSelected = false;
            EnableDisableReportTypeDropdown();
        }

        private void OnManageClicked(object parameter)
        {
            Console.WriteLine($"Manage Clicked : {parameter}");
            _navigationService.NavigateTo(Enums.PageKey.ManageDriver);
        }

        private void OnSystemClicked(object parameter)
        {
            Console.WriteLine($"System Clicked : {parameter}");
            _navigationService.NavigateTo(Enums.PageKey.System);
        }

        private void ExportReports(object parameter)
        {
            Console.WriteLine($"Selected Report Type : {SelectedReportType}, Export : {SelectedExportType}");
            string fileformats = SelectedExportType.ToLower() == "pdf" ? "PDF (*.pdf) | *.pdf" : "Excel (*.xlsx) | *.xlsx; *.xls";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = fileformats;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Title = $"Export as {SelectedExportType}";
            if (saveFileDialog.ShowDialog() == true)
            {
                var fileInfo = new FileInfo(saveFileDialog.FileName);
                Console.WriteLine($"Filename : {saveFileDialog.FileName}, {fileInfo.Name}, {fileInfo.Directory.FullName}");
                switch (SelectedReportType.ToLower())
                {
                    case "training report":
                        if (SelectedExportType.ToLower() == "excel")
                            Utility.ExportExcelReports.TrainingReport(Drivers.Where(x => x.IsSelected).ToList(), saveFileDialog.FileName);
                        else
                            ExportTrainingReportAsPDF(Drivers.Where(x => x.IsSelected).ToList(), saveFileDialog.FileName);
                        break;
                    case "individual report":
                        if (SelectedExportType.ToLower() == "excel")
                            Utility.ExportExcelReports.IndividualDriversReport(Drivers.Where(x => x.IsSelected).ToList(), saveFileDialog.FileName);
                        else
                            ExportIndividualReportsAsPDF(Drivers.Where(x => x.IsSelected).ToList(), fileInfo.Name, fileInfo.Directory.FullName);
                        break;
                    case "detailed lesson report":
                        if (SelectedExportType.ToLower() == "excel")
                            Utility.ExportExcelReports.DetailedLessonReport(Drivers.Where(x => x.IsSelected).ToList(), saveFileDialog.FileName);
                        else
                            ExportDetailedLessonReportsAsPDF(Drivers.Where(x => x.IsSelected).ToList(), fileInfo.Name, fileInfo.Directory.FullName);
                        break;
                }
            }
        }

        private IList<SelectableDriver> GetSelectableDrivers(IList<Driver> drivers)
        {
            IList<SelectableDriver> selectableDrivers = new List<SelectableDriver>();
            foreach (Driver driver in drivers)
            {
                IList<Lesson> excludedLessons = new List<Lesson>();
                foreach (Lesson lesson in SelectedProduct.Lessons)
                {
                    Console.WriteLine($"Attempts with Lesson : {lesson.LessonID} is Included {driver.Attempts.Any(x => x.LessonID == lesson.LessonID)}");
                    if (!driver.Attempts.Any(x => x.LessonID == lesson.LessonID))
                    {
                        excludedLessons.Add(lesson);
                        driver.Attempts.Add(new Attempt()
                        {
                            AttemptID = -1,
                            Lesson = lesson,
                            LessonID = lesson.LessonID,
                            DateCompleted = null,
                            IsComplete = false,
                            Infractions = null,
                            Score = 0,
                            TimeToComplete = 0,
                        });
                    }
                }

                Console.WriteLine($"Excluded Lessons : {excludedLessons}");
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
                    Attempts = driver.Attempts,
                    ProductID = driver.ProductID,
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

        private void ExportTrainingReportAsPDF(IList<SelectableDriver> drivers, string absoluteFileName)
        {
            //Utility.ExportPDFReports.TestTrainingReport();
            Utility.ExportPDFReports.TrainingReport(drivers, absoluteFileName);
        }

        private void ExportIndividualReportsAsPDF(IList<SelectableDriver> drivers, string filePrefix, string directory)
        {
            filePrefix = filePrefix.Replace(".pdf", string.Empty);
            foreach (SelectableDriver driver in drivers)
            {
                string fileName = $"{filePrefix}_{driver.DriverID}.pdf";
                Utility.ExportPDFReports.IndividualDriversReport(driver, $"{directory}/{fileName}");
            }
        }

        private void ExportDetailedLessonReportsAsPDF(IList<SelectableDriver> drivers, string filePrefix, string directory)
        {
            filePrefix = filePrefix.Replace(".pdf", string.Empty);
            foreach (SelectableDriver driver in drivers)
            {
                string fileName = $"{filePrefix}_{driver.DriverID}.pdf";
                Utility.ExportPDFReports.DetailedLessonReport(driver, $"{directory}/{fileName}");
            }
        }
    }
}
