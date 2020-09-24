using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Toute
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// Using <see cref="LoginPageViewModel"/> as ViewModel,
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

        }

        /// <summary>
        /// Constructor that accept view model as a parameter
        /// </summary>
        /// <param name="vm"></param>
        public RegisterPage(RegisterPageViewModel viewModel) : base(viewModel)
        {
            //Prepares Components for MainWindow
            InitializeComponent();
        }

        #endregion

    }
}
