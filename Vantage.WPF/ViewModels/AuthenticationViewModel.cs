using System;
using System.Security;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using Vantage.Common.Models;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Views;

namespace Vantage.WPF.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly DelegateCommand _loginCommand;
        private readonly DelegateCommand _logoutCommand;
        private readonly DelegateCommand _showViewCommand;

        public EventHandler OnRequestClose;
        public EventHandler OnRequestFocus;

        private string _username;
        private string _loggedInName;

        #region Properties
        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string LoggedInName 
        {
            get { return _loggedInName; }
            set { SetProperty(ref _loggedInName, value); }
        }

        public bool IsAuthenticated
        {
            get { return App.CurrentPrincipal != null ? App.CurrentPrincipal.Identity.IsAuthenticated : false; }
        }

        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                    return string.Format("Signed in as {0}. {1}",
                          App.CurrentPrincipal.Identity.Name,
                          App.CurrentPrincipal.IsInRole("Administrators") ? "You are an administrator!"
                              : "You are NOT a member of the administrators group.");

                return "Not authenticated!";
            }
        }
        #endregion

        #region Commands
        public DelegateCommand LoginCommand { get { return _loginCommand; } }

        public DelegateCommand LogoutCommand { get { return _logoutCommand; } }

        public DelegateCommand ShowViewCommand { get { return _showViewCommand; } }
        #endregion

        public AuthenticationViewModel(IAuthenticationService authenticationService, MainWindowViewModel mainWindowViewModel)
        {
            _authenticationService = authenticationService;
            _mainWindowViewModel = mainWindowViewModel;
            _loginCommand = new DelegateCommand(Login, CanLogin);
            _logoutCommand = new DelegateCommand(Logout, CanLogout);
            _showViewCommand = new DelegateCommand(ShowView, null);
        }

        private async void Login(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            string clearTextPassword = passwordBox.Password;
            try
            {
                IView dashboard = new Dashboard();
                // Allow user to login if username and password both empty
                if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(clearTextPassword))
                {
                    //dashboard.Show();

                    OnRequestClose?.Invoke(this, new EventArgs());
                    return;
                }

                App.Current.MainWindow.Cursor = Cursors.Wait;
                //Validate credentials through the authentication service
                UserReturnObject user = await _authenticationService.AuthenticateUser(Username, clearTextPassword);

                //Get the current principal object
                CustomPrincipal customPrincipal = App.CurrentPrincipal as CustomPrincipal;
                var currentPrincipal = Thread.CurrentPrincipal;

                if (customPrincipal == null)
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

                //Authenticate the user
                customPrincipal.Identity = new CustomIdentity(user.UserName, user.Roles);

                _mainWindowViewModel.IsLoggedIn = true;
                LoggedInName = $"{user.FirstName} {user.LastName}";

                //Update UI
                OnPropertyChanged("AuthenticatedUser");
                OnPropertyChanged("IsAuthenticated");

                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                Username = string.Empty; //reset
                passwordBox.Password = string.Empty; //reset
                _mainWindowViewModel.Status = string.Empty;
                App.Current.MainWindow.Cursor = Cursors.Arrow;

                //  dashboard.Show();

                OnRequestClose?.Invoke(this, new EventArgs());
            }
            catch (UnauthorizedAccessException)
            {
                App.Current.MainWindow.Cursor = Cursors.Arrow;

                _mainWindowViewModel.Status = "Please enter valid admin name and password.";
                passwordBox.Password = string.Empty;
                Username = string.Empty;
                OnRequestFocus?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                App.Current.MainWindow.Cursor = Cursors.Arrow;
                _mainWindowViewModel.Status = string.Format("ERROR: {0}", ex.Message);
            }
        }

        private bool CanLogin(object parameter)
        {
            return !IsAuthenticated;
        }

        private void Logout(object parameter)
        {
            CustomPrincipal customPrincipal = App.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                customPrincipal.Identity = new AnonymousIdentity();
                OnPropertyChanged("AuthenticatedUser");
                OnPropertyChanged("IsAuthenticated");
                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                _mainWindowViewModel.Status = string.Empty;
            }
        }

        private bool CanLogout(object parameter)
        {
            return IsAuthenticated;
        }

        private void ShowView(object parameter)
        {
            try
            {
                _mainWindowViewModel.Status = string.Empty;
                IView view = new Admin();
                //view.Show();
            }
            catch (SecurityException)
            {
                _mainWindowViewModel.Status = "You are not authorized!";
            }
        }
    }
}
