using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Vantage.WPF.Controls
{
    /// <summary>
    /// Interaction logic for TopTabbar.xaml
    /// </summary>
    public partial class TopTabbar : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IList<Models.TabItem>), typeof(TopTabbar), new UIPropertyMetadata(null, OnItemsSourceChanged));
        
        public IList<Models.TabItem> ItemsSource 
        {
            get { return (IList<Models.TabItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        //public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(TopTabbar));

        //public int SelectedIndex 
        //{
        //    get { return (int)GetValue(SelectedIndexProperty); }
        //    set { SetValue(SelectedIndexProperty, value); }
        //}

        public TopTabbar()
        {
            InitializeComponent();
        }

        private static void OnItemsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TopTabbar topTabbar = obj as TopTabbar;
            topTabbar.ICTabbar.ItemsSource = e.NewValue != null ? e.NewValue as IList<Models.TabItem> : null;
        }
    }
}
