using System.Security;

namespace Toute
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// Using <see cref="RegisterPageViewModel"/> as ViewModel,
    /// <see cref="BasePage"/> as base page, and 
    /// <see cref="IHavePassword"/> to handle password sending to VM
    /// </summary>
    public partial class RegisterPage : BasePage<RegisterPageViewModel>, IHaveDoublePassword
    {
        #region Public Members

        public SecureString FirstSecureString => MyPassword.SecurePassword;

        public SecureString SecondSecureString => MyConfirmPassword.SecurePassword;

        #endregion

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
        public RegisterPage(RegisterPageViewModel viewModel) : base(viewModel)
        {
            //Prepares Components for MainWindow
            InitializeComponent();
        }

        #endregion

    }
}
