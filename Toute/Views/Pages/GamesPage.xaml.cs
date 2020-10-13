namespace Toute
{
    /// <summary>
    /// Interaction logic for GamesPage.xaml
    /// Using <see cref="GamesViewModel"/> as ViewModel,
    /// <see cref="BasePage"/> as base page
    /// </summary>
    public partial class GamesPage : BasePage<GamesViewModel>
    {
        /// <summary>
        /// Default constructor of GamesPage
        /// </summary>
        public GamesPage()
        {
            //Prepares Components
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with viewModel given as parameter
        /// </summary>
        /// <param name="viewModel">ViewModel of GamesPage</param>
        public GamesPage(GamesViewModel viewModel) : base(viewModel)
        {
            //Prepares Components
            InitializeComponent();
        }
    }
}
