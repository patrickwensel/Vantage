using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vantage.WPF.Interfaces;
using Vantage.WPF.ViewModels;

namespace Vantage.WPF.Views
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "Administrators")]
    public partial class Admin : Page, IView
    {
        public Admin()
        {
            InitializeComponent();
        }

        #region IView Members
        public IViewModel ViewModel
        {
            get
            {
                return DataContext as IViewModel;
            }
            set
            {
                DataContext = value;
            }
        }
        #endregion
    }
}
