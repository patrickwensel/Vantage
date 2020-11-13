using System;
using System.Windows.Controls;
using Vantage.WPF.Interfaces;
using Vantage.WPF.ViewModels;

namespace Vantage.WPF.Views
{
    /// <summary>
    /// Interaction logic for ManageDriver.xaml
    /// </summary>
    public partial class ManageDriver : Page, IView
    {
        private readonly ManageDriverViewModel _manageDriverViewModel;

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        public ManageDriver(ManageDriverViewModel manageDriverViewModel)
        {
            _manageDriverViewModel = manageDriverViewModel;
            _manageDriverViewModel.OnErrorOccurred += OnErrorOccurred;
            ViewModel = _manageDriverViewModel;
            InitializeComponent();                       
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await _manageDriverViewModel.OnInitializedAsync();
        }

        private void OnErrorOccurred(object sender, EventArgs e)
        {
            if (_manageDriverViewModel.IsErrorInFirstName)
            {
                LblErrorMsg.Text = "First Name must be not null and contains alphabet only.";
                TxtFirstName.Focus();
                return;
            }

            if(_manageDriverViewModel.IsErrorInLastName)
            {
                LblErrorMsg.Text = "Last Name must be not null and contains alphabet only.";
                TxtLastName.Focus();
                return;
            }

            if(_manageDriverViewModel.IsErrorInUsername && _manageDriverViewModel.IsAddingDriver)
            {
                LblErrorMsg.Text = string.IsNullOrEmpty(_manageDriverViewModel.ErrorMessage) ? "Username must be not null and contains alphanumberic only." : _manageDriverViewModel.ErrorMessage; ;
                TxtUsername.Focus();
                return;
            }

            if(_manageDriverViewModel.IsErrorInPin)
            {
                LblErrorMsg.Text = "Pin number must be four digit only, no any aphabet allowed.";
                TxtPin.Focus();
                return;
            }

            if(_manageDriverViewModel.IsErrorInGroup)
            {
                LblErrorMsg.Text = "Please select any Group.";
                CBGroup.Focus();
                return;
            }
        }        
    }
}
