using Ninject;

namespace Toute
{
    /// <summary>
    /// IoC for our application
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// Kernel for our IoC
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        /// <summary>
        /// Setup application for using IoC
        /// </summary>
        public static void Setup()
        {
            //Binds all singleton view models to the application
            BindViewModels();
        }

        /// <summary>
        /// Binds all singleton view models
        /// </summary>
        private static void BindViewModels()
        {
            //Binds SettingsViewModel as singleton view model
            Kernel.Bind<SettingsViewModel>().ToConstant(new SettingsViewModel());

            //Binds ApplicationViewModel as singleton view model
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
        }
        /// <summary>
        /// Get a service form IoC of the specified type
        /// its shortcut for Kernel.Get<T>
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
