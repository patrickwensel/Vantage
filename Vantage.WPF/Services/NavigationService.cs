using System;
using System.Collections.Generic;
using System.Text;
using Vantage.WPF.Enums;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.Services
{
    public class NavigationService : INavigationService
    {
        public PageKey CurrentPageKey => throw new NotImplementedException();

        public void GoBack(int numberOfWindows = 0)
        {
            throw new NotImplementedException();
        }

        public void GoBackToPage(PageKey pageKey)
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(PageKey pageKey, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public void PopToRoot()
        {
            throw new NotImplementedException();
        }
    }
}
