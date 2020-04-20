using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vantage.WPF.Interfaces;
using Vantage.WPF.ViewModels;

namespace Vantage.WPF.Views
{
    /// <summary>
    /// Interaction logic for SystemView.xaml
    /// </summary>
    public partial class SystemView : Page, IView
    {
        private readonly SystemViewModel _systemViewModel;

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        public SystemView(SystemViewModel systemViewModel)
        {
            _systemViewModel = systemViewModel;
            ViewModel = _systemViewModel;
            _systemViewModel.ResetData += OnResetData;
            _systemViewModel.ErrorOccurred += OnErrorOccurred;
            InitializeComponent();            
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            BtnSubmit.IsEnabled = TxtPassword.Password == TxtConfirmPassword.Password;
        }

        private void OnResetData(object sender, EventArgs e)
        {
            TxtPassword.Password = null;
            TxtConfirmPassword.Password = null;
        }

        private void OnErrorOccurred(object sender, EventArgs e)
        {
            TxtUsername.Focus();
        }
    }
}
