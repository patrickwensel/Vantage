using System;
using System.Security;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Services;
using Vantage.WPF.Views;

namespace Vantage.WPF.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly DelegateCommand _loginCommand;
        private readonly DelegateCommand _logoutCommand;
        private readonly DelegateCommand _showViewCommand;

        public EventHandler OnRequestClose;
        public EventHandler OnRequestFocus;

        private string _username;
        private string _status;
        private bool _isLoggedIn = false;

        private bool _isLoginWindowVisible = true;

        #region Properties
        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
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

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public bool IsLoginWindowVisible
        {
            get { return _isLoginWindowVisible; }
            set { SetProperty(ref _isLoginWindowVisible, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand LoginCommand { get { return _loginCommand; } }

        public DelegateCommand LogoutCommand { get { return _logoutCommand; } }

        public DelegateCommand ShowViewCommand { get { return _showViewCommand; } }
        #endregion

        public AuthenticationViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
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
                    dashboard.Show();

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

                IsLoggedIn = true;
                //Update UI
                OnPropertyChanged("AuthenticatedUser");
                OnPropertyChanged("IsAuthenticated");

                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                Username = string.Empty; //reset
                passwordBox.Password = string.Empty; //reset
                Status = string.Empty;
                App.Current.MainWindow.Cursor = Cursors.Arrow;

                //  dashboard.Show();

                OnRequestClose?.Invoke(this, new EventArgs());
            }
            catch (UnauthorizedAccessException)
            {
                App.Current.MainWindow.Cursor = Cursors.Arrow;

                Status = "Please enter valid admin name and password.";
                passwordBox.Password = string.Empty;
                Username = string.Empty;
                OnRequestFocus?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                App.Current.MainWindow.Cursor = Cursors.Arrow;
                Status = string.Format("ERROR: {0}", ex.Message);
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
                Status = string.Empty;
            }
        }

        private bool CanLogout(object parameter)
        {
            return IsAuthenticated;
        }

        public bool IsAuthenticated
        {
            get { return App.CurrentPrincipal != null ? App.CurrentPrincipal.Identity.IsAuthenticated : false; }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                SetProperty(ref _isLoggedIn, value);
            }
        }

        private void ShowView(object parameter)
        {
            try
            {
                Status = string.Empty;
                IView view;
                if (parameter == null)
                    view = new SecretWindow();
                else
                    view = new AdminWindow();

                view.Show();
            }
            catch (SecurityException)
            {
                Status = "You are not authorized!";
            }
        }
    }
}
