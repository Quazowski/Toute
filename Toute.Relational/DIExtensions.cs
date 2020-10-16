using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Toute.Core;

namespace Toute.Relational
{
    public static class DIExtensions
    {
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
            }, ServiceLifetime.Transient);

            //Add scoped services to DI
            services.AddTransient<IClientDataStore>(provider =>
                new ClientDataStore(provider.GetService<ClientDataStoreDbContext>()));
        }
    }
}
