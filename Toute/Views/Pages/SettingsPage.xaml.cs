using System.Security;

namespace Toute
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// Using <see cref="SettingsViewModel"/> as ViewModel,
    /// <see cref="BasePage"/> as base page, and 
    /// </summary>
    public partial class SettingsPage : BasePage<SettingsViewModel>
    {
        /// <summary>
        /// Default constructor for <see cref="SettingsPage"/>
        /// </summary>
        public SettingsPage()
        {
            //Prepares Components
            InitializeComponent();
        }

        /// <summary>
        /// Constructor of <see cref="SettingsPage"/> with viewModel
        /// given as parameter
        /// </summary>
        /// <param name="viewModel">ViewModel for SettingsPage</param>
        public SettingsPage(SettingsViewModel viewModel) : base(viewModel)
        {
            //Prepares Components
            InitializeComponent();
        }
    }
}