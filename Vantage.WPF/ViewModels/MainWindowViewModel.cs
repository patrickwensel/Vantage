using System;
using System.Windows.Controls;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Messages;

namespace Vantage.WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMessagingService _messagingService;
        private readonly DelegateCommand _menuItemClickCommand;

        private string _status = string.Empty;
        private bool _isLoggedIn = false;
        private bool _isMenuItemClickInProgress;

        public DelegateCommand MenuItemClickCommand { get { return _menuItemClickCommand; } }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                SetProperty(ref _isLoggedIn, value);
            }
        }

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public MainWindowViewModel(INavigationService navigationService, IMessagingService messagingService)
        {
            _navigationService = navigationService;
            _messagingService = messagingService;
            _menuItemClickCommand = new DelegateCommand(MenuItemClicked, CanMenuItemClicked);
        }

        public void InitializeNavigationService(Frame frame)
        {
            _navigationService.Initialize(frame);
        }

        private void MenuItemClicked(object parameter)
        {
            _isMenuItemClickInProgress = true;
            string commandParameter = parameter as string;
            switch (commandParameter.ToLower())
            {
                case "exit":
                    _messagingService.Send<ExitAppMessage>(new ExitAppMessage());
                    break;
                case "login":
                    _navigationService.NavigateTo(Enums.PageKey.Login);
                    break;
                case "logout":
                    Console.WriteLine("Logout button pressed...");
                    break;
                case "training":
                    _navigationService.NavigateTo(Enums.PageKey.TrainingReport);
                    break;
                case "admin":
                    _navigationService.NavigateTo(Enums.PageKey.Admin);
                    break;
                default:
                    break;
            }
            _isMenuItemClickInProgress = false;
        }

        private bool CanMenuItemClicked(object parameter)
        {
            return !_isMenuItemClickInProgress;
        }
    }
}
