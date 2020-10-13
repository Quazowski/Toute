using NLog;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// View Model for GamesPage
    /// </summary>
    public class GamesViewModel : BaseViewModel
    {
        #region Private members

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public members

        /// <summary>
        /// All games that are added, and can be run
        /// </summary>
        public ObservableCollection<GameModel> Items { get; set; }

        /// <summary>
        /// Current item chosen in settings
        /// </summary>
        public string CurrentItemId { get; set; }

        /// <summary>
        /// Is settings popup open
        /// </summary>
        public bool SettingsPopupOpen { get; set; }

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

        /// <summary>
        /// Command that set new path for the game
        /// </summary>
        public ICommand SetNewValuesCommand { get; set; }


        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor for GamesPageViewModel
        /// </summary>
        public GamesViewModel()
        {
            _logger.Info("Start setting up GamesViewModel");

            //Creating new ObservableCollection to store Games
            Items = new ObservableCollection<GameModel>();

            //Create commands
            RunCommand = new ParametrizedRelayCommand((path) => RunGame((string)path));
            SettingsCommand = new ParametrizedRelayCommand((Id) => OpenSettingsGame((string)Id));
            RemoveGameCommand = new RelayCommand(DeleteGame);
            AddGameCommand = new RelayCommand(AddFile);
            SetNewValuesCommand = new RelayCommand(SetNewValues);

            _logger.Info("Done setting up GamesViewModel");
        }


        #endregion

        #region Helpers

        /// <summary>
        /// Adds a game to the application game search list
        /// </summary>
        private void AddFile()
        {
            _logger.Debug("Attempt to add a file");
            //Open a dialog and find a file u want to add...
            GameModel game = DialogHelpers.FindGame();

            //If file is not null...
            if (game != null)
            {
                //Add chosen file
                Items.Add(game);

                _logger.Debug("Added item to list of files, now attempt to add the file to LocalDB");

                SqliteDb.AddGameAsync(new GameDataModel 
                {
                    Id = game.FileId,
                    Path = game.Path,
                    Title = game.Title,
                    UserId = ViewModelApplication.ApplicationUser.Id,
                    Image = game.BytesImage
                });

                _logger.Debug("Saved file to LocalDB");
            }
            else
            {
                _logger.Debug("Not found any file to add");
            }
        }

        /// <summary>
        /// Deletes a game from whole application
        /// </summary>
        private void DeleteGame()
        {
            _logger.Debug("Attempt to delete a file");
            var itemToDelete = Items.FirstOrDefault(x => x.FileId == CurrentItemId);
            //Make popup close
            SettingsPopupOpen = false;

            //Remove chosen item
            Items.Remove(itemToDelete);

            _logger.Debug("Deleted file from a list of items, now attempt to delete a file from LocalDB");
            SqliteDb.RemoveGameAsync(itemToDelete.FileId);

            _logger.Debug("Deleted item from LocalDB");
        }

        /// <summary>
        /// Method that handle starting chosen game
        /// </summary>
        private void RunGame(string path)
        {
            _logger.Debug($"Attempt to run a file of path: {path}");
            //Run a file with given path
            new Process().RunFile(path);

            _logger.Debug("File is run");
        }

        /// <summary>
        /// Method that handle open settings of chosen game
        /// </summary>
        private void OpenSettingsGame(string Id)
        {
            //Set Current item to chosen item
            CurrentItemId = Items.FirstOrDefault(x => x.FileId == Id).FileId;

            //Toggle popup
            SettingsPopupOpen ^= true;
            _logger.Debug($"Changed visibility of Settings of File to {SettingsPopupOpen}");
        }

        private void SetNewValues()
        {
            _logger.Debug($"Attempt to change values of file with id: {CurrentItemId}");
            //Open a dialog and find a file u want to add...
            var newValues = DialogHelpers.SetNewValueForFile();

            //Find file from the list, and change his values
            var file = Items.FirstOrDefault(x => x.FileId == CurrentItemId);
            file.Path = newValues.Path;
            file.BytesImage = newValues.BytesImage;
            file.Title = newValues.Title;

            _logger.Debug($"Changed item from list, now trying to change file in LocalDB");

            //Save a new path in db
            SqliteDb.ChangeValuesAsync(new GameDataModel
            {
                Id = CurrentItemId,
                Image = file.BytesImage,
                Path = file.Path,
                Title = file.Title
            });

            _logger.Debug($"File values changed successfully");
        }

        #endregion

    }
}
