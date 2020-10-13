using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Toute.Core;
using Toute.Relational;

namespace Toute
{
    /// <summary>
    /// Static extensions for <see cref="IServiceCollection"/> to add new services
    /// </summary>
    public static class DIExtensions
    {
        /// <summary>
        /// Extensions that adds all static ViewModels that are used in application
        /// </summary>
        /// <param name="services"></param>
        public static void AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<ApplicationViewModel>();
            services.AddSingleton<SideMenuViewModel>();
            services.AddSingleton<GamesViewModel>();
        }

        /// <summary>
        /// Extensions that adds SQliteDB
        /// </summary>
        /// <param name="services"></param>
        public static void AddSqliteDb(this IServiceCollection services)
        {
            //Add a DB to our application
            services.AddDbContext<ClientDataStoreDbContext>(options =>
            {
                options.UseSqlite(CoreDI.Configuration.GetConnectionString("ClientDataStoreConnection"));
            });

            //Add scoped services to DI
            services.AddScoped<IClientDataStore>(provider =>
                new ClientDataStore(provider.GetService<ClientDataStoreDbContext>()));
        }
    }
}
