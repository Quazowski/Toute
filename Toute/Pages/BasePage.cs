using System.Windows.Controls;

namespace Toute
{
    public class BasePage : Page
    {
        public BasePage()
        {
            Loaded += BasePage_Loaded;
        }

        private async void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await this.AddSlideAndFadeAnimation(PageAnimation.RightSlides, false);
        }
    }

    public class BasePage<VM> : BasePage where VM : BaseViewModel, new()
    {
        private VM viewModel;
        public VM ViewModel 
        {
            get => viewModel;
            set
            {
                if (ViewModel == value)
                    return;

                viewModel = value;
            }

        }
        public BasePage() : base()
        {

        }

        public BasePage(VM model) : base()
        {
            viewModel = model;

            DataContext = viewModel;
        }
    }
}
