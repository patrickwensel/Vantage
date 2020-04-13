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
            ViewModel = _manageDriverViewModel;
            InitializeComponent();                       
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await _manageDriverViewModel.OnInitializedAsync();
        }
    }
}
