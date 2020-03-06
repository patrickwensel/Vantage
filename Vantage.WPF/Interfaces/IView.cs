using System;
using System.Collections.Generic;
using System.Text;
using Vantage.WPF.ViewModels;

namespace Vantage.WPF.Interfaces
{
    public interface IView
    {
        IViewModel ViewModel
        {
            get;
            set;
        }

        void Show();
    }
}
