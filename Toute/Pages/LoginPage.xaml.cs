using System.Security;

namespace Toute
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// Using <see cref="LoginPageViewModel"/> as ViewModel,
    /// <see cref="BasePage"/> as base page, and 
    /// <see cref="IHavePassword"/> to handle password sending to VM
    /// </summary>
    public partial class LoginPage : BasePage<LoginPageViewModel>, IHavePassword
    {
        #region Public Members

        /// <summary>
        /// Secure string to handle sending password to ViewModel
        /// </summary>
        public SecureString SecureString => MyPassword.SecurePassword;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginPage()
        {

        }

        /// <summary>
        /// Constructor that accept view model as a parameter
        /// </summary>
        /// <param name="vm"></param>
        public LoginPage(LoginPageViewModel vm) : base(vm)
        {
            //Prepares Components for MainWindow
            InitializeComponent();
        }

        #endregion

    }
}
