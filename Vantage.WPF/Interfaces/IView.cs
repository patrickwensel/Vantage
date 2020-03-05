using System;
using System.Collections.Generic;
using System.Text;

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
