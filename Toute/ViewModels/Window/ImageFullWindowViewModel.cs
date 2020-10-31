using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Toute
{
    public class ImageFullWindowViewModel : BaseViewModel
    {
        private ImageFullWindow _window;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ICommand MaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ImageFullWindowViewModel(ImageFullWindow window)
        {
            _logger.Debug("ImageFullWindowViewModel is started to set up");

            _window = window;

            //create commands
            MaximizeCommand = new RelayCommand(Maximize);
            CloseCommand = new RelayCommand(Close);

            _logger.Debug("ImageFullWindowViewModel done set up");
        }
        private void Maximize()
        {
            //Maximize window
            _window.WindowState ^= WindowState.Maximized;
            _logger.Debug("Window is Maximized");
        }

        private void Close()
        {
            //Close application
            _window.Close();
            _logger.Debug("Window is closed");
        }
    }
}
