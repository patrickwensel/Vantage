using System;
using System.Collections.Generic;
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
        private readonly ICommand _groupSelectedCommand;
        private readonly ICommand _manageCommand;
        private readonly ICommand _systemCommand;

        private int _fetchedDriversCount;
        private Group _selectedGroup;
        private UserInfo _loggedInUserInfo;
        private IList<Group> _groups;
        private IList<Driver> _drivers;
        private IList<TabItem> _tabItems;

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

        public IList<TabItem> TabItems 
        {
            get { return _tabItems; }
            set { SetProperty(ref _tabItems, value); }
        }

        public ICommand GroupSelectedCommand { get { return _groupSelectedCommand; } }

        public TrainingReportViewModel(IGroupService groupService, IDriverService driverService, MainWindowViewModel mainWindowViewModel)
        {
            _groupService = groupService;
            _driverService = driverService;
            LoggedInUserInfo = mainWindowViewModel.LoggedInUserInfo;
            _groupSelectedCommand = new DelegateCommand(OnGroupSelected);
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
            await FetchGroupsAsync();
            await FetchDriversAsync();
        }

        public async Task FetchDriversAsync()
        {
            Drivers = await _driverService.GetAllDrivers();

            FetchedDriversCount = Drivers != null ? Drivers.Count : 0;
            foreach(Driver driver in Drivers)
            {
                Console.WriteLine($"Drivers : {driver.GroupedAttemptsByLessons}");
            }
        }

        public async Task FetchGroupsAsync()
        {
            Groups = await _groupService.GetGroups();
            Console.WriteLine($"Groups : {Groups}");
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
