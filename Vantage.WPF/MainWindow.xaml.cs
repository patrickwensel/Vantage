using System.Windows;
using Vantage.WPF.Interfaces;
using Vantage.WPF.ViewModels;

namespace Vantage.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView
    {
        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            ViewModel = mainWindowViewModel;
            InitializeComponent();
            mainWindowViewModel.InitializeNavigationService(FrmContentArea);
        }
    }
}
