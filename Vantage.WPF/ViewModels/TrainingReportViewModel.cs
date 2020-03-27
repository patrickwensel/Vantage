using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Vantage.Common.Models;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.ViewModels
{
    public class TrainingReportViewModel : BaseViewModel, IViewModel
    {
        private readonly IGroupService _groupService;
        private readonly IDriverService _driverService;
        private readonly ICommand _groupSelectedCommand;

        private int _fetchedDriversCount;
        private Group _selectedGroup;
        private IList<Group> _groups;
        private IList<Driver> _drivers;

        public int FetchedDriversCount
        {
            get { return _fetchedDriversCount; }
            private set { SetProperty(ref _fetchedDriversCount, value); }
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

        public ICommand GroupSelectedCommand { get { return _groupSelectedCommand; } }

        public TrainingReportViewModel(IGroupService groupService, IDriverService driverService)
        {
            _groupService = groupService;
            _driverService = driverService;
            _groupSelectedCommand = new DelegateCommand(OnGroupSelected);
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

            Console.WriteLine($"Drivers : {Drivers}");
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
    }
}
