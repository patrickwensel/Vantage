using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Unity.Resolution;
using Vantage.Common;
using Vantage.WPF.Enums;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Views;

namespace Vantage.WPF.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<Type, PageKey> _pages;

        private Frame _mainFrame = null;

        public PageKey CurrentPageKey => throw new NotImplementedException();

        public NavigationService()
        {
            _pages = new Dictionary<Type, PageKey>();
        }

        public void Initialize(Frame frame)
        {
            if (_mainFrame != null)
                throw new TypeInitializationException("Main frame already initialized", null);

            _mainFrame = frame;
        }

        public void GoBack(int numberOfWindows = 1)
        {
            if (_mainFrame == null)
                throw new TypeInitializationException("Main Frame not initialized, please call NavigationService.Initialize() First", null);

            while(numberOfWindows > 0)
            {
                if (_mainFrame.CanGoBack)
                    _mainFrame.GoBack();
            }
        }

        public void GoBackToPage(PageKey pageKey)
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(PageKey pageKey, IDictionary<string, object> parameters = null)
        {
            if (_mainFrame == null)
                throw new TypeInitializationException("Main Frame not initialized, please call NavigationService.Initialize() First", null);

            Page page = GetPage(pageKey, parameters);
            _mainFrame.Navigate(page);
            Console.WriteLine($"Name : {_mainFrame.Name}, CurrentSource : {_mainFrame.CurrentSource}, {_mainFrame.BackStack}");
        }

        public void PopToRoot()
        {
            if (_mainFrame == null)
                throw new TypeInitializationException("Main Frame not initialized, please call NavigationService.Initialize() First", null);

            while (_mainFrame.CanGoBack)
                _mainFrame.GoBack();
        }

        private Page GetPage(PageKey pageKey, IDictionary<string, object> parameters = null)
        {
            Page page;
            ResolverOverride[] resolverOverrides = null;
            if (parameters != null && parameters.Count() > 0)
            {
                resolverOverrides = new ResolverOverride[parameters.Count()];
                for (int i = 0; i < parameters.Count(); i++)
                {
                    var dictionaryItem = parameters.ElementAt(i);
                    resolverOverrides[i] = new ParameterOverride(dictionaryItem.Key, dictionaryItem.Value);
                }
            }

            switch (pageKey)
            {
                case PageKey.Login:
                    page = (Page)ContainerManager.Container.Resolve(typeof(Login), typeof(Login).ToString());
                    break;
                case PageKey.Dashboard:
                    page = (Page)ContainerManager.Container.Resolve(typeof(Dashboard), typeof(Dashboard).ToString());
                    break;
                case PageKey.TrainingReport:
                    page = (Page)ContainerManager.Container.Resolve(typeof(TrainingReport), typeof(TrainingReport).ToString());
                    break;
                case PageKey.Admin:
                    page = (Page)ContainerManager.Container.Resolve(typeof(Admin), typeof(Admin).ToString());
                    break;
                default:
                    throw new ArgumentException(
                        $"No such page: {pageKey}. Did you forgot to register your view in Bootstrapper?", nameof(pageKey));
            }

            if (!_pages.ContainsKey(page.GetType()))
                _pages.Add(page.GetType(), pageKey);

            return page;
        }
    }
}
