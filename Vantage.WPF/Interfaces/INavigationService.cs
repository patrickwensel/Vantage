using System;
using System.Collections.Generic;
using System.Text;
using Vantage.WPF.Enums;

namespace Vantage.WPF.Interfaces
{
    public interface INavigationService
    {
        PageKey CurrentPageKey { get; }

        void NavigateTo(PageKey pageKey, IDictionary<string, object> parameters);

        void GoBack(int numberOfWindows = 0);

        void GoBackToPage(PageKey pageKey);

        void PopToRoot();
    }
}
