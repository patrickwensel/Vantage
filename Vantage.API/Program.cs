using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unity.Microsoft.DependencyInjection;
using Vantage.Common;
using Vantage.Data;

namespace Vantage.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WireupdDependency();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseUnityServiceProvider(ContainerManager.Container)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        private static void WireupdDependency()
        {
            ContainerManager.Initialize();
            CommonBootstrapper.Initialize(ContainerManager.Container);
            DataBootstrapper.Initialize(ContainerManager.Container);
            APIBootstrapper.Initialize(ContainerManager.Container);
        }
    }
}
