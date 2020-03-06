using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Vantage.WPF.Interfaces;
using Vantage.WPF.ViewModels;
using Vantage.WPF.Views;

namespace Vantage.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            //Create a custom principal with an anonymous identity at startup
            CustomPrincipal customPrincipal = new CustomPrincipal();

            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);
            Thread.CurrentPrincipal = customPrincipal;
            var currentPrincipal = Thread.CurrentPrincipal;
            AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.UnauthenticatedPrincipal);

            base.OnStartup(e);

            //Show the login view
            AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
            IView loginWindow = new LoginWindow(viewModel);
            loginWindow.Show();

        }
    }
}
