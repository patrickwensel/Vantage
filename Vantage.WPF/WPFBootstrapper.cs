using Microsoft.Extensions.Configuration;
using System.IO;
using Unity;
using Unity.Lifetime;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Services;
using Vantage.WPF.ViewModels;
using Vantage.WPF.Views;

namespace Vantage.WPF
{
    public class WPFBootstrapper
    {
        public static void Initialize(IUnityContainer container)
        {
            // WPF Dependency registration goes here
            RegisterServices(container);
            RegisterViewModels(container);
            RegisterViews(container);
        }

        private static void RegisterViewModels(IUnityContainer container)
        {
            container.RegisterType<MainWindowViewModel>(new HierarchicalLifetimeManager());
            container.RegisterType<AuthenticationViewModel>(new TransientLifetimeManager());
            container.RegisterType<TrainingReportViewModel>(new TransientLifetimeManager());
            container.RegisterType<AdminViewModel>(new TransientLifetimeManager());
            container.RegisterType<DashboardViewModel>(new TransientLifetimeManager());
        }

        private static void RegisterViews(IUnityContainer container)
        {
            container.RegisterType<MainWindow>(new TransientLifetimeManager());
            container.RegisterType<Login>(new TransientLifetimeManager());
            container.RegisterType<TrainingReport>(new TransientLifetimeManager());
            container.RegisterType<Dashboard>(new TransientLifetimeManager());
            container.RegisterType<Admin>(new TransientLifetimeManager());
        }

        private static void RegisterServices(IUnityContainer container)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = configBuilder.Build();

            container.RegisterInstance<IConfiguration>(config);
            container.RegisterType<IMessagingService, MessagingService>(new HierarchicalLifetimeManager());
            container.RegisterType<INavigationService, NavigationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProductService, ProductService>(new TransientLifetimeManager());
            container.RegisterType<IAuthenticationService, AuthenticationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAttemptService, AttemptService>(new TransientLifetimeManager());
            container.RegisterType<IDriverService, DriverService>(new TransientLifetimeManager());
            container.RegisterType<IGroupService, GroupService>(new TransientLifetimeManager());
        }
    }
}
