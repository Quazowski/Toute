using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Toute.Core;
using Toute.Relational;

namespace Toute
{
    public static class DIExtensions
    {
        public static void AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<ApplicationViewModel>();
            services.AddSingleton<SideMenuViewModel>();
            services.AddSingleton<SettingsViewModel>();
        }
        public static void AddSqliteDb(this IServiceCollection services)
        {
            services.AddDbContext<ClientDataStoreDbContext>(options =>
            {
                options.UseSqlite(CoreDI.Configuration.GetConnectionString("ClientDataStoreConnection"));
            });

            services.AddScoped<IClientDataStore>(provider =>
                new ClientDataStore(provider.GetService<ClientDataStoreDbContext>()));
        }
    }
}
