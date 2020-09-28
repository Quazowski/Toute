using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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

        /// <summary>
        /// Current item chosen in settings
        /// </summary>
        public GameModel CurrentItem { get; set; }

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

        /// <summary>
        /// Command that delete game with given path
        /// </summary>
        public ICommand RemoveGameCommand { get; set; }

        /// <summary>
        /// Command that add game
        /// </summary>
        public ICommand AddGameCommand { get; set; }


        #endregion

        #region Constructor

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
            SettingsCommand = new ParametrizedRelayCommand((path) => OpenSettingsGame((string)path));

            //Command that delete game with given path
            RemoveGameCommand = new RelayCommand(DeleteGame);

            //Command that add game
            AddGameCommand = new RelayCommand(AddGame);

            //Loads all files that were added
            var items = FileManaging.ReadAllTextFilesFromFolder("Files");

            //For every file, that were added.... 
            foreach (var file in items)
            {
                //Add to Item list a GameModel
                Items.Add(JsonSerializer.Deserialize<GameModel>(file));
            }

        }

        #endregion

        #region Helpers

        /// <summary>
        /// Adds a game to the application game search list
        /// </summary>
        private void AddGame()
        {
            //Open a dialog and find a file u want to add...
            GameModel game = DialogExtensions.FindGame();

            //If file is not null...
            if (game != null)
                //Add chosen file
                Items.Add(game);

        }

        /// <summary>
        /// Deletes a game from whole application
        /// </summary>
        private void DeleteGame()
        {
            //Make popup close
            CurrentItem.PopupOpen = false;

            //Remove chosen item
            Items.Remove(CurrentItem);

            //Run deleting of file and photo async, to prevent blocked UI or thread
            Task.Run(async () =>
            {
                //Wait 100ms to be sure thread won't be block
                await Task.Delay(100);

                //Delete file of game
                FileManaging.DeleteFile(CurrentItem.PathToFile);

                //Delete photo of game
                FileManaging.DeleteFile(CurrentItem.FullPathToImage);
            });


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
        private void OpenSettingsGame(string path)
        {
            //Set Current item to chosen item
            CurrentItem = Items.FirstOrDefault(x => x.PathToGame == path);

            //Toggle popup
            CurrentItem.PopupOpen ^= true;

        }

        #endregion

    }
}
