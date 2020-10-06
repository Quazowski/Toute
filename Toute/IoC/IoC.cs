using Ninject;

namespace Toute
{
    /// <summary>
    /// IoC for our application
    /// </summary>
    public static class IoC
    {
        #region Public properties

        /// <summary>
        /// Kernel for our IoC
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        #endregion

        #region Setup

        /// <summary>
        /// Setup application for using IoC
        /// </summary>
        public static void Setup()
        {
            //Binds all singleton view models to the application
            BindViewModels();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Binds all singleton view models
        /// </summary>
        private static void BindViewModels()
        {
            //Binds ApplicationViewModel as singleton view model
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());

            //Binds GamesPageViewModel as singleton view model 
            Kernel.Bind<GamesPageViewModel>().ToConstant(new GamesPageViewModel());

            //Binds SideMenuViewModel as singleton view model 
            Kernel.Bind<SideMenuViewModel>().ToConstant(new SideMenuViewModel());
        }

        /// <summary>
        /// Get a service form IoC of the specified type
        /// its shortcut for Kernel.Get<T>
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>Returns ViewModel of specified type</returns>
        public static T Get<T>()
        {
            //Returns ViewModel of specified type
            return Kernel.Get<T>();
        }

        #endregion


    }
}
