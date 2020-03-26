using System;
using System.Windows;
using System.Windows.Controls;
using Vantage.WPF.Interfaces;
using Vantage.WPF.ViewModels;

namespace Vantage.WPF.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page, IView
    {
        public Login(AuthenticationViewModel viewModel)
        {
            viewModel.OnRequestFocus += RequestFocus;
            ViewModel = viewModel;
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            TxtUsername.Focus();
        }

        #region IView Members
        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
        #endregion

        private void RequestFocus(object sender, EventArgs e)
        {
            TxtUsername.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
