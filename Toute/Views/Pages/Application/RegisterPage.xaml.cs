namespace Toute
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// Using <see cref="RegisterViewModel"/> as ViewModel,
    /// <see cref="BasePage"/> as base page, and 
    /// <see cref="IHavePassword"/> to handle password sending to VM
    /// </summary>
    public partial class RegisterPage : BasePage<RegisterViewModel>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisterPage()
        {
            //Prepares Components for MainWindow
            InitializeComponent();
        }

        /// <summary>
        /// Constructor that accept view model as a parameter
        /// </summary>
        /// <param name="vm">ViewModel for RegisterPage</param>
        public RegisterPage(RegisterViewModel viewModel) : base(viewModel)
        {
            //Prepares Components for MainWindow
            InitializeComponent();
        }

        #endregion

    }
}
