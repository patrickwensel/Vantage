using System;
using System.Collections.Generic;
using System.Windows.Input;
using Vantage.WPF.Controls.Models;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.ViewModels
{
    public class SystemViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly ICommand _trainingCommand;
        private readonly ICommand _manageCommand;

        private UserInfo _loggedInUserInfo;
        private IList<TabItem> _tabItems;

        public UserInfo LoggedInUserInfo
        {
            get { return _loggedInUserInfo; }
            set { SetProperty(ref _loggedInUserInfo, value); }
        }

        public IList<TabItem> TabItems
        {
            get { return _tabItems; }
            set { SetProperty(ref _tabItems, value); }
        }

        public SystemViewModel(INavigationService navigationService, MainWindowViewModel mainWindowViewModel)
        {
            _navigationService = navigationService;
            _mainWindowViewModel = mainWindowViewModel;
            LoggedInUserInfo = mainWindowViewModel.LoggedInUserInfo;
            _trainingCommand = new DelegateCommand(OnTrainingClicked);
            _manageCommand = new DelegateCommand(OnManageClicked);
            TabItems = new List<TabItem>()
            {
                new TabItem() { Icon = "", Text = "Training Reports", IsSelected = false, ClickCommand = _trainingCommand },
                new TabItem() { Icon = "", Text = "Manage Drivers", IsSelected = false, ClickCommand = _manageCommand },
                new TabItem() { Icon = "", Text = "System", IsSelected = true, ClickCommand = null },
            };
        }

        private void OnTrainingClicked(object parameter)
        {
            Console.WriteLine($"Training Clicked : {parameter}");
            _navigationService.NavigateTo(Enums.PageKey.TrainingReport);
        }

        private void OnManageClicked(object parameter)
        {
            Console.WriteLine($"Manage Clicked : {parameter}");
            _navigationService.NavigateTo(Enums.PageKey.ManageDriver);

        }
    }
}
