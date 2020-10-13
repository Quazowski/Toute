using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Toute.Core
{
    public static class CoreDI
    {
        public static IServiceProvider ServiceProvider;
        public static IServiceCollection Services;
        public static IConfiguration Configuration;

        public static IHostBuilder ConfigureDI()
        {
            IHostBuilder host = Host.CreateDefaultBuilder().ConfigureAppConfiguration((context, builder) => { builder.AddJsonFile("appsettings.json", optional: false); });

            return host;
        }
        public static IHost BuildDI(this IHostBuilder hostBuilder)
        {
            var host = hostBuilder.Build();
            ServiceProvider = host.Services;
            
            return host;

        }

        public static T Service<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}
