using Microsoft.Extensions.DependencyInjection;
using Toute.Core;

namespace Toute
{
    /// <summary>
    /// Static class that will get basic services from DI, and make them static
    /// </summary>
    public static class DI
    {
        public static ApplicationViewModel ViewModelApplication => CoreDI.ServiceProvider.GetService<ApplicationViewModel>();
        public static SideMenuViewModel ViewModelSideMenu => CoreDI.ServiceProvider.GetService<SideMenuViewModel>();
        public static GamesViewModel ViewModelGame => CoreDI.ServiceProvider.GetService<GamesViewModel>();
        public static IClientDataStore SqliteDb => CoreDI.Service<IClientDataStore>();
    }
}
