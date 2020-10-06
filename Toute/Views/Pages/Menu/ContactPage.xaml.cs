namespace Toute
{
    /// <summary>
    /// Interaction logic for ContactPage.xaml
    /// Using <see cref="ContactPageViewModel"/> as ViewModel,
    /// <see cref="BasePage"/> as base page, and 
    /// </summary>
    public partial class ContactPage : BasePage<ContactPageViewModel>
    {
        /// <summary>
        /// Default constructor for <see cref="ContactPage"/>
        /// </summary>
        public ContactPage()
        {
            //Prepares Components
            InitializeComponent();
        }

        /// <summary>
        /// Constructor of <see cref="ContactPage"/> with viewModel
        /// given as parameter
        /// </summary>
        /// <param name="viewModel">ViewModel for ContactPage</param>
        public ContactPage(ContactPageViewModel viewModel) : base(viewModel)
        {
            //Prepares Components
            InitializeComponent();
        }
    }
}