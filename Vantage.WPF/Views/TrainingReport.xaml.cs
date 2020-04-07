using System;
using System.Windows.Controls;
using System.Windows.Media;
using Vantage.WPF.Helpers;
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

        //private void DataGridCell_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    DataGridCell cell = sender as DataGridCell;
        //    Console.WriteLine($"Clicked Cell : {cell}");
        //    if (cell != null && !cell.IsEditing)
        //    {
        //        DataGridRow row = VisualHelper.FindParent<DataGridRow>(cell);
        //        if (row != null)
        //        {
        //            CheckBox checkBox = VisualHelper.FindChild<CheckBox>(cell);

        //            if (checkBox != null)
        //            {
        //                HitTestResult result = VisualTreeHelper.HitTest(checkBox, e.GetPosition(cell));

        //                if (result != null)
        //                {
        //                    // execute button and do not select / deselect row
        //                    checkBox.Command.Execute(row.DataContext);
        //                    e.Handled = true;
        //                    return;
        //                }
        //            }

        //            row.IsSelected = !row.IsSelected;
        //            e.Handled = true;
        //        }
        //    }
        //}
    }
}
