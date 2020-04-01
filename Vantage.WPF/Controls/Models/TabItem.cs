using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Vantage.WPF.Controls.Models
{
    public class TabItem
    {
        public string Icon { get; set; }

        public string Text { get; set; }

        public bool IsSelected { get; set; }

        public ICommand ClickCommand { get; set; }
    }
}
