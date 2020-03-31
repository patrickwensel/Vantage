using System;
using System.Windows;
using System.Windows.Controls;
using Vantage.WPF.Interfaces;
using Vantage.WPF.ViewModels;

namespace Vantage.WPF.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page, IView
    {
        private readonly DashboardViewModel _dashboardViewModel;

        public Dashboard(DashboardViewModel dashboardViewModel)
        {
            ViewModel = dashboardViewModel;
            _dashboardViewModel = dashboardViewModel;
            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await _dashboardViewModel.OnInitializedAsync();
        }

        #region IView Members
        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
        #endregion

        private void openAdmin_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
