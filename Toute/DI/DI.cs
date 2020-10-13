using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Toute.Core;

namespace Toute
{
    public static class DI
    {
        public static ApplicationViewModel ViewModelApplication => CoreDI.ServiceProvider.GetService<ApplicationViewModel>();
        public static SideMenuViewModel ViewModelSideMenu => CoreDI.ServiceProvider.GetService<SideMenuViewModel>();
        public static GamesViewModel ViewModelGame => CoreDI.ServiceProvider.GetService<GamesViewModel>();
        public static IClientDataStore SqliteDb => CoreDI.Service<IClientDataStore>();
    }
}
