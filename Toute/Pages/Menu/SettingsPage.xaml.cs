using System.Security;

namespace Toute
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// Using <see cref="SettingsPageViewModel"/> as ViewModel,
    /// <see cref="BasePage"/> as base page, and 
    /// </summary>
    public partial class SettingsPage : BasePage<SettingsPageViewModel>
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
        public SettingsPage(SettingsPageViewModel viewModel) : base(viewModel)
        {
            //Prepares Components
            InitializeComponent();
        }
    }
}