using System.Windows.Controls;

namespace Toute
{
    /// <summary>
    /// Class that will be used by all pages
    /// This class will implement base behavior for pages
    /// </summary>
    public class BasePage : Page
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage()
        {
            //If page is loaded fire BasePage_Loaded event
            Loaded += BasePage_Loaded;
        }

        #endregion

        #region Helper Methods/Events

        /// <summary>
        /// Method that will be fired by BasePage_Loaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //All incoming pages will fade in
            await this.AddFadeAnimation(false);
        }

        #endregion
    }

    /// <summary>
    /// BasePage<typeparamref name="VM"/> that inherent from <see cref="BasePage"/>
    /// This class will be used to create page of specified ViewModel
    /// </summary>
    /// <typeparam name="VM">ViewModel that page will be using</typeparam>
    public class BasePage<VM> : BasePage where VM : BaseViewModel, new()
    {
        #region Private members

        /// <summary>
        /// The view model associated  with this page
        /// </summary>
        private VM viewModel;

        #endregion

        #region Public members

        /// <summary>
        /// The view model associated  with this page
        /// </summary>
        public VM ViewModel
        {
            get => viewModel;
            set
            {
                //If nothing changed, return...
                if (ViewModel == value)
                    return;

                //Set new viewModel
                viewModel = value;
            }

        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor that interests from base constructor
        /// </summary>
        public BasePage() : base()
        {

        }

        /// <summary>
        /// Constructor that accept viewModel as a parameter
        /// and interests from base constructor
        /// </summary>
        /// <param name="model"></param>
        public BasePage(VM model) : base()
        {
            //Sets new ViewModel for a Page of given model
            ViewModel = model;

            //Sets a DataContext of new Page to given ViewModel
            DataContext = ViewModel;
        }
        #endregion
    }
}
