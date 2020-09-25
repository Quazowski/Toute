using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Toute
{
    /// <summary>
    /// View Model for GamesPage
    /// </summary>
    public class GamesPageViewModel : BaseViewModel
    {
        #region Public members

        /// <summary>
        /// All games that are added, and can be run
        /// </summary>
        public ObservableCollection<GameModel> Items { get; set; }

        public bool PopupHidden { get; set; } = true;

        #endregion

        #region Public commands

        /// <summary>
        /// Command to run chosen game
        /// </summary>
        public ICommand RunCommand { get; set; }

        /// <summary>
        /// Command to open settings of chosen game
        /// </summary>
        public ICommand SettingsCommand { get; set; }


        #endregion

        /// <summary>
        /// Default Constructor for GamesPageViewModel
        /// </summary>
        public GamesPageViewModel()
        {
            //Creating new ObservableCollection to store Games
            Items = new ObservableCollection<GameModel>();

            //Command to run chosen game
            RunCommand = new ParametrizedRelayCommand((path) => RunGame((string)path));

            //Command to open settings of chosen game
            SettingsCommand = new RelayCommand(OpenSettingsGame);

            Items.Add(new GameModel { Title = "Config Folder", PathToGame = @"C:\Users\wardl\Desktop\config" });
            Items.Add(new GameModel { Title = "22", PathToGame = "D..." });
            Items.Add(new GameModel { Title = "22", PathToGame = "D..." });
            Items.Add(new GameModel { Title = "22", PathToGame = "D..." });
            Items.Add(new GameModel { Title = "22", PathToGame = "D..." });
            Items.Add(new GameModel { Title = "22", PathToGame = "D..." });
        }

        /// <summary>
        /// Method that handle starting chosen game
        /// </summary>
        private void RunGame(string path)
        {
            //Run a file with given path
            new Process().RunFile(path);
        }

        /// <summary>
        /// Method that handle open settings of chosen game
        /// </summary>
        private void OpenSettingsGame()
        {
            PopupHidden ^= true;
        }
    }
}
