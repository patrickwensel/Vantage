using System;
using System.Collections.Generic;
using System.Windows.Input;
using Vantage.Common.Models;
using Vantage.WPF.Controls.Models;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.ViewModels
{
    public class SystemViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INavigationService _navigationService;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly ICommand _trainingCommand;
        private readonly ICommand _manageCommand;
        private readonly ICommand _createBackupCommand;
        private readonly ICommand _restoreBackupCommand;
        private readonly ICommand _resetCommand;
        private readonly ICommand _updateCredentialCommand;

        private UserInfo _loggedInUserInfo;
        private IList<TabItem> _tabItems;
        private string _userName;
        private string _errorMessage;

        public EventHandler ResetData;
        public EventHandler ErrorOccurred;

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

        public string Username
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        public ICommand CreateBackupCommand { get { return _createBackupCommand; } }

        public ICommand RestoreBackupCommand { get { return _restoreBackupCommand; } }

        public ICommand ResetCommand { get { return _resetCommand; } }

        public ICommand UpdateCredentialCommand { get { return _updateCredentialCommand; } }

        public SystemViewModel(INavigationService navigationService, IAuthenticationService authenticationService, MainWindowViewModel mainWindowViewModel)
        {
            _navigationService = navigationService;
            _authenticationService = authenticationService;
            _mainWindowViewModel = mainWindowViewModel;
            LoggedInUserInfo = mainWindowViewModel.LoggedInUserInfo;
            Username = mainWindowViewModel.LoggedInUserInfo.FullUserInfo.UserName;
            _trainingCommand = new DelegateCommand(OnTrainingClicked);
            _manageCommand = new DelegateCommand(OnManageClicked);
            TabItems = new List<TabItem>()
            {
                new TabItem() { Icon = "", Text = "Training Reports", IsSelected = false, ClickCommand = _trainingCommand },
                new TabItem() { Icon = "", Text = "Manage Drivers", IsSelected = false, ClickCommand = _manageCommand },
                new TabItem() { Icon = "", Text = "System", IsSelected = true, ClickCommand = null },
            };

            _createBackupCommand = new DelegateCommand(CreateBackup);
            _restoreBackupCommand = new DelegateCommand(RestoreBackup);
            _resetCommand = new DelegateCommand(ResetCredential);
            _updateCredentialCommand = new DelegateCommand(UpdateCredential);
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

        private void CreateBackup(object parameter)
        {

        }

        private void RestoreBackup(object parameter)
        {

        }

        private void ResetCredential(object parameter)
        {
            Username = null;
            ResetData?.Invoke(this, new EventArgs());
        }

        private async void UpdateCredential(object parameter)
        {
            System.Windows.Controls.PasswordBox passwordBox = parameter as System.Windows.Controls.PasswordBox;
            App.SetCursorToWait();
            User user = await _authenticationService.GetUserByUsername(Username);
            if (user != null && user.UserID != _mainWindowViewModel.LoggedInUserInfo.FullUserInfo.UserId)
            {
                App.SetCursorToArrow();
                ErrorMessage = "Username already exists, Please try Another.";
                ErrorOccurred?.Invoke(this, new EventArgs());
                return;
            }
            
            await _authenticationService.UpdateCredential(new User()
            {
                UserID = _mainWindowViewModel.LoggedInUserInfo.FullUserInfo.UserId,
                UserName = this.Username,
                FirstName = _mainWindowViewModel.LoggedInUserInfo.FullUserInfo.FirstName,
                LastName = _mainWindowViewModel.LoggedInUserInfo.FullUserInfo.LastName,
                Password = passwordBox.Password,
            });
            App.SetCursorToArrow();
        }
    }
}
