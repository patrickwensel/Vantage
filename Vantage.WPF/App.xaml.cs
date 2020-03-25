using System;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Vantage.Common;

namespace Vantage.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IPrincipal CurrentPrincipal;

        protected override void OnStartup(StartupEventArgs e)
        {
            WireupDependency();

            MainWindow mainWindow = (MainWindow)ContainerManager.Container.Resolve(typeof(MainWindow), typeof(MainWindow).ToString());
            
            //Create a custom principal with an anonymous identity at startup
            CustomPrincipal customPrincipal = new CustomPrincipal();

            CurrentPrincipal = customPrincipal;
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);
            Thread.CurrentPrincipal = customPrincipal;
            AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.UnauthenticatedPrincipal);

            mainWindow.Show();

            base.OnStartup(e);
        }

        private void WireupDependency()
        {
            ContainerManager.Initialize();
            CommonBootstrapper.Initialize(ContainerManager.Container);
            WPFBootstrapper.Initialize(ContainerManager.Container);
        }
    }
}
