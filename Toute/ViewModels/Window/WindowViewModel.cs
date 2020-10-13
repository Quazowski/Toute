using NLog;
using System.Windows;
using System.Windows.Input;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// ViewModel of MainWidow
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {
        #region Private members
        /// <summary>
        /// Main Window
        /// </summary>
        private readonly Window _window;

        /// <summary>
        /// Private thickness of <see cref="DropShadowBorderPadding"/>
        /// To change when windowState is changed
        /// </summary>
        private Thickness _dropShadowBorderPadding = new Thickness(10);

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public members

        /// <summary>
        /// Padding of the Toute Title text box
        /// </summary>
        public Thickness HeaderTitlePadding { get; set; } = new Thickness(25, 10, 25, 10);
        
        /// <summary>
        /// Padding of drop shadow border
        /// Changes when windowState is changed
        /// </summary>
        public Thickness DropShadowBorderPadding
        {
            get
            {
                return _window.WindowState == WindowState.Maximized ? new Thickness(0) : _dropShadowBorderPadding;
            }
            set
            {
                _dropShadowBorderPadding = value;
            }
        }

        /// <summary>
        /// The thickness of resizeBorder
        /// Should be bigger than dropShadowBorderPadding plus
        /// value of the resizable border
        /// </summary>
        public Thickness ResizeBorderThickness => new Thickness(DropShadowBorderPadding.Bottom + 2);

        /// <summary>
        /// Font size of the header Title
        /// </summary>
        public int HeaderFontSize { get; set; } = 48;

        /// <summary>
        /// Value of capitonHeight
        /// </summary>
        public int CaptionHeight => HeaderFontSize + (int)HeaderTitlePadding.Top + (int)HeaderTitlePadding.Bottom;

        /// <summary>
        /// Overlay over application
        /// </summary>
        public bool OverlayVisible { get; set; } = false;
        #endregion

        #region Commands

        /// <summary>
        /// Command to minimize Window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// Command to maximize Window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// Command to close application
        /// </summary>
        public ICommand CloseCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowViewModel(Window window)
        {
            _logger.Info("Start setting up WindowViewModel");

            //Assign main window to private member
            _window = window;

            //Event that is fired every time when State of Changed
            //to change values of paddings etc.
            _window.StateChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(DropShadowBorderPadding));
            };

            //Create commands
            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeCommand = new RelayCommand(Maximize);
            CloseCommand = new RelayCommand(Close);

            _logger.Info("Done setting up WindowViewModel");
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Private method that is fired when minimize button is clicked
        /// </summary>
        private void Minimize()
        {
            //Minimize window
            //_window.WindowState = WindowState.Minimized;
            ViewModelApplication.InformationsAndErrors.Add(new InfoControlViewModel
            {
                Message = "test",
                IsError = true
            });

            _logger.Debug("Window is Minimized");
        }

        /// <summary>
        /// Private method that is fired when Maximize button is clicked
        /// </summary>
        private void Maximize()
        {
            //Maximize window
            _window.WindowState ^= WindowState.Maximized;
            _logger.Debug("Window is Maximized");
        }

        /// <summary>
        /// Private method that is fired when close button is clicked
        /// </summary>
        private void Close()
        {
            //Close application
            _window.Close();
            _logger.Info("Application is shutting down...");
            Application.Current.Shutdown();
        }

        #endregion
    }
}
