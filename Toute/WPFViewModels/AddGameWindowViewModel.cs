using System;
using System.Windows;

namespace Toute
{
    public class AddGameWindowViewModel : WindowViewModel
    {
        public AddGameWindowViewModel(AddGameWindow window) : base(window)
        {
            DropShadowBorderPadding = new Thickness(10);

            //CloseCommand = new RelayCommand(ClosePopup(window));
        }

        private Action ClosePopup(AddGameWindow window)
        {
            window.Close();

            return null;
        }
    }
}
