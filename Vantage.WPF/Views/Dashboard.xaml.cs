using System.Windows;
using System.Windows.Controls;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page, IView
    {
        public Dashboard()
        {
            InitializeComponent();
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
