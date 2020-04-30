using iTextSharp.text.pdf.parser;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Vantage.Common.Models;
using Vantage.WPF.Controls.Models;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.ViewModels
{
    public class SystemViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly IDatabaseService _databaseService;
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
        private string _successMessage;

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

        public string SuccessMessage 
        {
            get { return _successMessage; }
            set { SetProperty(ref _successMessage, value); }
        }

        public ICommand CreateBackupCommand { get { return _createBackupCommand; } }

        public ICommand RestoreBackupCommand { get { return _restoreBackupCommand; } }

        public ICommand ResetCommand { get { return _resetCommand; } }

        public ICommand UpdateCredentialCommand { get { return _updateCredentialCommand; } }

        public SystemViewModel(INavigationService navigationService, IUserService userService, IDatabaseService databaseService, MainWindowViewModel mainWindowViewModel)
        {
            _navigationService = navigationService;
            _userService = userService;
            _databaseService = databaseService;
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

        private async void CreateBackup(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Database Backup (*.bak) | *.bak ";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Title = $"Backup Database";
            if (saveFileDialog.ShowDialog() == false)
            {
                return;
            }

            App.SetCursorToWait();
            var backupResponse = await _databaseService.BackupDatabase(saveFileDialog.FileName.Trim());
            App.SetCursorToArrow();

            if (backupResponse != null && backupResponse.Result)
            {
                System.Windows.MessageBox.Show("Database backup created successfully.", "Backup", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            else if(backupResponse != null)
            {
                System.Windows.MessageBox.Show(backupResponse.Message, "Backup Failed!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            else
            {
                System.Windows.MessageBox.Show("Backup Failed, Please make sure you are working on local database.", "Backup Failed!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async void RestoreBackup(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Database Backup (*.bak) | *.bak ";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Title = $"Restore Database";            
            if (openFileDialog.ShowDialog() == false)
            {
                return;
            }

            App.SetCursorToWait();
            var restoreResponse = await _databaseService.RestoreDatabase(openFileDialog.FileName.Trim());
            App.SetCursorToArrow();
            if (restoreResponse != null && restoreResponse.Result)
            {
                System.Windows.MessageBox.Show("Database restored successfully.", "Restore", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            else if(restoreResponse != null)
            {
                System.Windows.MessageBox.Show(restoreResponse.Message, "Restore Failed!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            else
            {
                System.Windows.MessageBox.Show("Restore failed, Please make sure you are working on local database.", "Restore Failed!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void ResetCredential(object parameter)
        {
            Username = null;
            ResetData?.Invoke(this, new EventArgs());
        }

        private async void UpdateCredential(object parameter)
        {
            System.Windows.Controls.PasswordBox passwordBox = parameter as System.Windows.Controls.PasswordBox;
            SuccessMessage = null;
            ErrorMessage = null;
            App.SetCursorToWait();
            User user = await _userService.GetUserByUsername(Username);

            // user might enter empty username, in such case above method return user object with default property values.
            if (user != null && user.UserID == 0)
            {
                IList<User> users = await _userService.GetUsers();
                user = users.FirstOrDefault(x => x.UserName == this.Username);
            }

            if (user != null && user.UserID != _mainWindowViewModel.LoggedInUserInfo.FullUserInfo.UserId)
            {
                App.SetCursorToArrow();                
                ErrorMessage = "Username already exists, Please try Another.";
                ErrorOccurred?.Invoke(this, new EventArgs());
                return;
            }

            await _userService.UpdateCredential(new User()
            {
                UserID = _mainWindowViewModel.LoggedInUserInfo.FullUserInfo.UserId,
                UserName = this.Username,
                FirstName = _mainWindowViewModel.LoggedInUserInfo.FullUserInfo.FirstName,
                LastName = _mainWindowViewModel.LoggedInUserInfo.FullUserInfo.LastName,
                Password = passwordBox.Password,
            });
            App.SetCursorToArrow();
            if(_mainWindowViewModel.LoggedInUserInfo.FullUserInfo.UserName.Equals(this.Username))
            {
                SuccessMessage = "Password updated successfully";
            }
            else
            {
                _mainWindowViewModel.LoggedInUserInfo.FullUserInfo.UserName = this.Username;
                SuccessMessage = "Username and Password updated successfully";
            }            
        }
    }
}
