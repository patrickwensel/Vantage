using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Vantage.WPF.Interfaces;
using Vantage.WPF.ViewModels;

namespace Vantage.WPF.Views
{
    /// <summary>
    /// Interaction logic for TrainingReport.xaml
    /// </summary>
    public partial class TrainingReport : Page, IView
    {
        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        public TrainingReport(TrainingReportViewModel trainingReportViewModel)
        {
            ViewModel = trainingReportViewModel;
            InitializeComponent();
        }

        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await (ViewModel as TrainingReportViewModel).OnInitializedAsync();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine($"Selected Item : {e.Source}");
        }
    }
}
