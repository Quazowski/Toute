using System.Threading.Tasks;
using System.Windows.Controls;

namespace Toute
{
    /// <summary>
    /// Interaction logic for InfoControl.xaml
    /// </summary>
    public partial class InfoControl : UserControl
    {
        public InfoControl()
        {
            InitializeComponent();

            Loaded += InfoControl_Loaded;
        }

        private async void InfoControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await this.AddSlideAndFadeAnimation(PageAnimation.TopSlides, false, keepMargin: false);

            await Task.Delay(5000);

            await this.AddSlideAndFadeAnimation(PageAnimation.TopSlides, true, keepMargin: false);
        }
    }
}
