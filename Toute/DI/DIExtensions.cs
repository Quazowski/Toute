using Microsoft.Extensions.DependencyInjection;

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
    }
}
