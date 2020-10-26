using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Toute.Core;
using Toute.Extensions;
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

        private string _nameOfFiles = "";

        #endregion

        #region Public members

        /// <summary>
        /// All files that are added, and can be run
        /// </summary>
        public ObservableCollection<GameModel> Items { get; set; }

        /// <summary>
        /// Filtered files by name. By default this is shown on view.
        /// </summary>
        public ObservableCollection<GameModel> FilteredItems { get; set; }

        /// <summary>
        /// Current item chosen in settings
        /// </summary>
        public string CurrentItemId { get; set; }

        /// <summary>
        /// Name of files that will be shown
        /// </summary>
        public string NameOfFiles {
            get => _nameOfFiles;
            set 
            {
                _nameOfFiles = value;
                SearchValueChanged();
            }
        }

        /// <summary>
        /// Is settings popup open
        /// </summary>
        public bool SettingsPopupOpen { get; set; }

        /// <summary>
        /// Indicate if searching for files with specific name is open
        /// </summary>
        public bool LookingForFilesClosed { get; set; } = true;

        /// <summary>
        /// Indicate if add file is running
        /// </summary>
        public bool AddFileIsRunning { get; set; }

        /// <summary>
        /// Indicate if remove game is running
        /// </summary>
        public bool RemoveGameIsRunning { get; set; }

        /// <summary>
        /// Indicate if change values running
        /// </summary>
        public bool SetNewIconAndPathIsRunning { get; set; }

        /// <summary>
        /// Indicate if search for... is running
        /// </summary>
        public bool QuickSearchIsRunning { get; set; }

        /// <summary>
        /// Indicate if Add Multi Launch is running
        /// </summary>
        public bool AddMultiLaunchIsRunning { get; set; }

        /// <summary>
        /// Indicate if <see cref="SetNewTitleAsync"/> is running
        /// set this to true
        /// </summary>
        public bool SetNewTitleIsRunning { get; set; }

        /// <summary>
        /// Indicate if TextBox that show user he have no games is shown
        /// If true, its hidden
        /// </summary>
        public bool NoGamesHiddenTxt { get; set; }

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

        /// <summary>
        /// Command to open menu for search multiple .exe files
        /// </summary>
        public ICommand QuickSearchCommand { get; set; }

        /// <summary>
        /// Command to add multi launch
        /// </summary>
        public ICommand AddMultiLaunchCommand { get; set; }

        /// <summary>
        /// Command to set new title for a file
        /// </summary>
        public ICommand SetNewTitleCommand { get; set; }

        /// <summary>
        /// Filter from <see cref="Items"/> files by name
        /// </summary>
        public ICommand SearchFilesOpenCommand { get; set; }

        /// <summary>
        /// Command to close or clear TextBox that are responsible for
        /// filtering a files
        /// </summary>
        public ICommand TextBoxCommand { get; set; }


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

            Items.CollectionChanged += (sender, e) =>
            {
                NoGamesHiddenTxt = Items.Count > 0;
            };

            //Create commands
            RunCommand = new ParametrizedRelayCommand((Id) => RunGame((string)Id));
            SettingsCommand = new ParametrizedRelayCommand((Id) => OpenSettingsGame((string)Id));
            RemoveGameCommand = new RelayCommand(async () => await DeleteGame());
            AddGameCommand = new RelayCommand(async () => await AddFile());
            SetNewValuesCommand = new RelayCommand(async () => await SetNewIconAndPath());
            QuickSearchCommand = new RelayCommand(() => QuickSearch());
            AddMultiLaunchCommand = new RelayCommand(async () => await AddMultiLaunch());
            SetNewTitleCommand = new RelayCommand(async () => await SetNewTitleAsync());
            SearchFilesOpenCommand = new RelayCommand(() => ManageVisibleOfSearch());
            TextBoxCommand = new ParametrizedRelayCommand((txt) => ClearCloseTextBox(txt));

            _logger.Info("Done setting up GamesViewModel");
        }


        #endregion

        #region Command methods

        private void ClearCloseTextBox(object txt)
        {
            var textBox = (txt as TextBox);
            
            if(string.IsNullOrEmpty(textBox.Text))
            {
                LookingForFilesClosed = true;
            }
            else
            {
                textBox.Text = "";
            }
        }

        /// <summary>
        /// Opens/Close search box
        /// </summary>
        private void ManageVisibleOfSearch()
        {
            LookingForFilesClosed ^= true;
        }

        /// <summary>
        /// Method to set new title for a file
        /// </summary>
        private async Task SetNewTitleAsync()
        {
            await RunCommandAsync(() => SetNewTitleIsRunning, async () =>
            {
                _logger.Debug($"Attempt to change title of file with id: {CurrentItemId}");

                NewNameDialog dialog = new NewNameDialog();
                dialog.ShowDialog();

                var newName = (dialog.DataContext as NewNameDialogViewModel).NewName;

                if (string.IsNullOrEmpty(newName) || newName?.Length <= 1)
                    return;

                //Find file from the list, and change his values
                var file = Items.FirstOrDefault(x => x.FileId == CurrentItemId);
                file.Title = newName;

                _logger.Debug($"Changed item from list, now trying to change file title in LocalDB");

                //Save a new path in db
                await SqliteDb.ChangeValuesAsync(new GameDataModel
                {
                    Id = CurrentItemId,
                    Title = file.Title
                });

                _logger.Debug($"File values changed successfully");
            });
        }

        /// <summary>
        /// Method to add multi launch
        /// </summary>
        /// <returns>Task when finish</returns>
        private async Task AddMultiLaunch()
        {
            await RunCommandAsync(() => AddMultiLaunchIsRunning, async () =>
            {
                _logger.Debug("Attempt to add new multi file");

                //Open dialog to select files
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;
                dialog.ShowDialog();
                if (string.IsNullOrEmpty(dialog.FileName))
                    return;

                //Create and show new AddMultipleFilesPopup
                var addMultiFiles = new AddMultipleFilesDialog();
                addMultiFiles.ShowDialog();

                //Get dialog DataContext
                var configurationOfFile = (addMultiFiles.DataContext as AddMultipleFilesDialogViewModel);

                //If user choose save option....
                if (configurationOfFile.SavedSucesfully && configurationOfFile.NameForLaunch.Length >= 2)
                {
                    List<StringDataModel> paths = new List<StringDataModel>();

                    //Add every path to the list of paths
                    foreach (var path in dialog.FileNames)
                    {
                        paths.Add(new StringDataModel { Value = path });
                    }

                    //Create a model of file
                    var files = new GameDataModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = ViewModelApplication?.ApplicationUser?.Id,
                        Image = configurationOfFile.ImageInBytes,
                        Title = configurationOfFile.NameForLaunch,
                        Paths = paths
                    };

                    //Add model to the item list, that user can see
                    Items.Add(new GameModel
                    {
                        FileId = files.Id,
                        Paths = (List<StringDataModel>)files.Paths,
                        Title = files.Title,
                        BytesImage = files.Image
                    });

                    //Save game to DB
                    await SqliteDb.AddGameAsync(files);
                    _logger.Debug("Multi file was added successfully");
                }
                else
                {
                    PopupExtensions.NewInfoPopup("Failed to add multi launcher");
                    _logger.Debug("No new multi file was added");
                }

            });
        }

        /// <summary>
        /// Method to open quick search window
        /// </summary>
        private void QuickSearch()
        {
            //Create and open quickSerarchDialog
            QuickSearchDialog dialog = new QuickSearchDialog();
            dialog.ShowDialog();
        }

        /// <summary>
        /// Adds a game to the application game search list
        /// </summary>
        private async Task AddFile()
        {
            await RunCommandAsync(() => AddFileIsRunning, async () =>
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

                    await SqliteDb.AddGameAsync(new GameDataModel
                    {
                        Id = game.FileId,
                        Paths = new List<StringDataModel>{ game.Paths.FirstOrDefault() },
                        Title = game.Title,
                        UserId = ViewModelApplication?.ApplicationUser?.Id,
                        Image = game.BytesImage
                    });

                    _logger.Debug("Saved file to LocalDB");
                }
                else
                {
                    _logger.Debug("Not found any file to add");
                }
            });
        }

        /// <summary>
        /// Deletes a game from whole application
        /// </summary>
        private async Task DeleteGame()
        {
            await RunCommandAsync(() => RemoveGameIsRunning, async () =>
            {
                _logger.Debug("Attempt to delete a file");
                var itemToDelete = Items.FirstOrDefault(x => x.FileId == CurrentItemId);
                //Make popup close
                SettingsPopupOpen = false;

                //Remove chosen item
                Items.Remove(itemToDelete);

                _logger.Debug("Deleted file from a list of items, now attempt to delete a file from LocalDB");
                await SqliteDb.RemoveGameAsync(itemToDelete.FileId);

                _logger.Debug("Deleted item from LocalDB");
            });
        }

        /// <summary>
        /// Method that handle starting chosen game
        /// </summary>
        private void RunGame(string Id)
        {
            _logger.Debug($"Attempt to run a file of Id: {Id}");

            var Paths = Items.FirstOrDefault(x => x.FileId == Id).Paths;

            if (Paths == null)
                return;

            //Run all file with given path
            foreach (var path in Paths)
            {
                new Process().RunFile(path.Value);
            }

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

        private async Task SetNewIconAndPath()
        {
            await RunCommandAsync(() => SetNewIconAndPathIsRunning, async () =>
            {
                _logger.Debug($"Attempt to change values of file with id: {CurrentItemId}");
                //Open a dialog and find a file u want to add...
                var newValues = DialogHelpers.SetNewIconAndPathForFile();

                if (newValues == null)
                    return;

                //Find file from the list, and change his values
                var file = Items.FirstOrDefault(x => x.FileId == CurrentItemId);
                file.Paths = newValues.Paths;
                file.BytesImage = newValues.BytesImage;

                _logger.Debug($"Changed item from list, now trying to change file in LocalDB");

                //Save a new path in db
                await SqliteDb.ChangeValuesAsync(new GameDataModel
                {
                    Id = CurrentItemId,
                    Image = file.BytesImage,
                    Paths = new List<StringDataModel> { file.Paths.FirstOrDefault() },
                });

                _logger.Debug($"File values changed successfully");
            });
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Method to search for a specific items from list
        /// </summary>

        private void SearchValueChanged()
        {
            //If there is no value...
            if (string.IsNullOrEmpty(NameOfFiles))
            {
                //Get all values
                FilteredItems = new ObservableCollection<GameModel>(Items);
            }
            //Otherwise...
            else
            {
                //Get filtered items
                FilteredItems = new ObservableCollection<GameModel>(Items.Where(x => x.Title.ToLower().Contains(NameOfFiles.ToLower())));
            }
        }

        #endregion

    }
}
