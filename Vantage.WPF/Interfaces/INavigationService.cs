using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Vantage.WPF.Enums;

namespace Vantage.WPF.Interfaces
{
    public interface INavigationService
    {
        PageKey CurrentPageKey { get; }

        void Initialize(Frame frame);

        void NavigateTo(PageKey pageKey, IDictionary<string, object> parameters = null);

        void GoBack(int numberOfWindows = 1);

        void GoBackToPage(PageKey pageKey);

        void PopToRoot();
    }
}
