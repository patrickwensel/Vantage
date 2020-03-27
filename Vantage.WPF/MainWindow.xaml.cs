using System;
using System.ComponentModel;
using System.Windows;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Messages;
using Vantage.WPF.ViewModels;

namespace Vantage.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView
    {
        private readonly IMessagingService _messagingService;

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        public MainWindow(MainWindowViewModel mainWindowViewModel, IMessagingService messagingService)
        {
            ViewModel = mainWindowViewModel;
            _messagingService = messagingService;
            InitializeComponent();
            mainWindowViewModel.InitializeNavigationService(FrmContentArea);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            _messagingService.Subscribe<ExitAppMessage>(this, OnExitAppMessageReceived);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _messagingService.Unsubscribe<ExitAppMessage>(this);
        }

        private void OnExitAppMessageReceived(object sender, ExitAppMessage exitAppMessage)
        {
            Console.WriteLine("ExitAppMessage Received");
            this.Close();
        }
    }
}
