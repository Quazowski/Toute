using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Toute.Core;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// ViewModel for a <see cref="QuickSearchDialog"/>
    /// </summary>
    public class QuickSearchViewModel : WindowViewModel
    {
        #region Private members

        /// <summary>
        /// Logger for <see cref="QuickSearchViewModel"/>
        /// </summary>
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Cancellation token to end not needed task
        /// </summary>
        private CancellationTokenSource _tokenSource = null;

        #endregion

        #region Public members
        /// <summary>
        /// Files that are find for the launcher
        /// </summary>
        public ObservableCollection<GameModel> Items { get; set; }

        public bool CheckBoxState { get; set; }

        /// <summary>
        /// Indicate if <see cref="SteamGamesAsync"/> is running
        /// </summary>
        public bool LookForSteamGamesIsRunning { get; set; }

        /// <summary>
        /// Indicate if <see cref="BattleNetAsync"/> is running
        /// </summary>
        public bool LookForBattleNetGamesIsRunning { get; set; }

        /// <summary>
        /// Is true if any of looking for games is running
        /// </summary>
        public bool Loading { get; set; } = false;

        /// <summary>
        /// Indicate if <see cref="AddFilesAsync(Window)"/> is running
        /// </summary>
        public bool AddFilesIsRunning { get; set; }

        /// <summary>
        /// Visibility of warning text
        /// </summary>
        public bool LongTimeTextVisibility { get; set; } = true;

        /// <summary>
        /// Gets logical drives 
        /// </summary>
        public int LogicalDrivesOnPc => Directory.GetLogicalDrives().Count();

        /// <summary>
        /// Amount of disc that are checked for a files.
        /// </summary>
        public int ProgressOfSearching { get; set; }

        #endregion

        #region Public commands

        /// <summary>
        /// Command to cancel adding by quick search
        /// </summary>
        public ICommand CancelQuickSearchCommand { get; set; }

        /// <summary>
        /// Command to add selected files by quick search
        /// </summary>
        public ICommand AddFilesCommand { get; set; }

        /// <summary>
        /// Command to find all steam games
        /// </summary>
        public ICommand SteamGamesCommand { get; set; }

        /// <summary>
        /// Command to find all battle.net games
        /// </summary>
        public ICommand BattleNetGamesCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor that pass window as parameter
        /// </summary>
        /// <param name="window">AddGameWindow</param>
        public QuickSearchViewModel(Window window) : base(window)
        {
            _logger.Info("Start setting up QuickSearchViewModel");

            Items = new ObservableCollection<GameModel>();

            //Change DropShadowBorderPadding to 10
            DropShadowBorderPadding = new Thickness(10);

            //Change HeaderFontSize to 18, to make capitation height smaller
            HeaderFontSize = 18;

            //Create commands
            CloseCommand = new RelayCommand(() => ClosePopup(window));
            CancelQuickSearchCommand = new RelayCommand(() => ClosePopup(window));
            AddFilesCommand = new RelayCommand(async() => await AddFilesAsync(window));
            SteamGamesCommand = new RelayCommand(async() => await SteamGamesAsync());
            BattleNetGamesCommand = new RelayCommand(async() => await BattleNetAsync());

            _logger.Info("Done setting up QuickSearchViewModel");
        }

        #region Command Methods

        /// <summary>
        /// Method to find all steam games from the PC
        /// </summary>
        private async Task SteamGamesAsync()
        {
            await RunCommandAsync(() => LookForSteamGamesIsRunning, async() => 
            {
                if(!Loading)
                {
                    //Prepare parameters...
                    ProgressOfSearching = 0;
                    _tokenSource = new CancellationTokenSource();
                    var token = _tokenSource.Token;
                    Loading = true;
                    LongTimeTextVisibility = false;
                    Items = new ObservableCollection<GameModel>();

                    //Create helper lists
                    List<string> ExeFiles = new List<string>();
                    List<Task> LookingForFilesTasks = new List<Task>();

                    //For each drive
                    foreach (var drive in Directory.GetLogicalDrives())
                    {
                        //Run a task to...
                        LookingForFilesTasks.Add(Task.Run(() =>
                        {
                            //Add .exe files to the list
                            GetExeFromDirectory(drive.ToString(), ExeFiles, @"\STEAMAPPS\COMMON", token);
                        }).ContinueWith((t1) => ProgressOfSearching++));
                    }
                    
                    //When all .exe files was find
                    await Task.WhenAll(LookingForFilesTasks);

                    //Clear for null files
                    ExeFiles = ExeFiles.Where(x => x != null).ToList();

                    //Get information about every file, and add to the list
                    foreach (var exeFile in ExeFiles)
                    {
                        var file = new FileInfo(exeFile);

                        Items.Add(new GameModel
                        {
                            FileId = Guid.NewGuid().ToString(),
                            Title = file.Name.Substring(0, file.Name.IndexOf('.')),
                            Paths = new List<StringDataModel> { new StringDataModel {Value = file.FullName}},
                            BytesImage = Icon.ExtractAssociatedIcon(exeFile).IconToBytes()
                        });
                    }
                    Loading = false;
                }
            });
        }

        private async Task BattleNetAsync()
        {
            await RunCommandAsync(() => LookForBattleNetGamesIsRunning, async () =>
            {
                if(!Loading)
                {
                    //Prepare parameters...
                    ProgressOfSearching = 0;
                    Loading = true;
                    LongTimeTextVisibility = false;
                    Items = new ObservableCollection<GameModel>();
                    _tokenSource = new CancellationTokenSource();
                    var token = _tokenSource.Token;

                    //Create helper lists
                    List<string> ExeFiles = new List<string>();
                    List<Task> LookingForFilesTasks = new List<Task>();

                    //For each drive
                    foreach (var drive in Directory.GetLogicalDrives())
                    {
                        //Run a task to...
                        LookingForFilesTasks.Add(Task.Run(() =>
                        {
                            //Add .exe files to the list
                            var paths = StringHelpers.FindPathToExe(drive,
                                new string[] { "BlackOpsColdWar",
                                            "BlackOps4",
                                            "ModernWarfare",
                                            "Hearthstone",
                                            "Heroes of the Storm",
                                            "Overwatch",
                                            "StarCraft II",
                                            "Warcraft III",
                                            "Wow",
                                            "Diablo III"}, token);

                            foreach (var path in paths)
                            {
                                ExeFiles.Add(path);
                            }
                        }).ContinueWith((t1) => ProgressOfSearching++));
                    }

                    //When all .exe files was find
                    await Task.WhenAll(LookingForFilesTasks);

                    //Clear for null files
                    ExeFiles = new List<string>(ExeFiles.Where(x => x != null).ToList().Distinct());

                    //Get information about every file, and add to the list
                    foreach (var exeFile in ExeFiles)
                    {
                        var file = new FileInfo(exeFile);

                        Items.Add(new GameModel
                        {
                            FileId = Guid.NewGuid().ToString(),
                            Title = file.Name.Substring(0, file.Name.IndexOf('.')),
                            Paths = new List<StringDataModel> { new StringDataModel {Value = file.FullName } },
                            BytesImage = Icon.ExtractAssociatedIcon(exeFile).IconToBytes()
                        });
                    }
                    Loading = false;
                }
            });
        }
        #endregion

        /// <summary>
        /// Method to add selected files
        /// </summary>
        private async Task AddFilesAsync(Window window)
        {
            await RunCommandAsync(() => AddFilesIsRunning, async() =>
            {
                //for each game that is selected...
                foreach (var file in Items.Where(x => x.IsSelected == true))
                {
                    _logger.Debug("Attempt to add a file");

                    //If file is not null...
                    if (file != null)
                    {
                        //Add chosen file to list of games
                        ViewModelGame.Items.Add(file);

                        _logger.Debug("Added item to list of files, now attempt to add the file to LocalDB");

                        //Add game to DB
                        await SqliteDb.AddGameAsync(new GameDataModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            Paths = new List<StringDataModel> { file.Paths.FirstOrDefault() },
                            Title = file.Title,
                            UserId = ViewModelApplication?.ApplicationUser?.Id,
                            Image = file.BytesImage
                        });

                        _logger.Debug("Saved file to LocalDB");
                    }
                    else
                    {
                        _logger.Debug("Not found any file to add");
                    }
                }

                ClosePopup(window);
            });

        }

        /// <summary>
        /// Method that Close Window
        /// </summary>
        /// <param name="window">AddGameWindow</param>
        private void ClosePopup(Window window)
        {
            _logger.Debug("Try to close popup window");

            //Cancel all running tasks
            if (_tokenSource != null)
                _tokenSource.Cancel();

            //Close popup window
            window.Close();

            _logger.Debug("Closed popup window");
        }

        #endregion

        #region Helper methods
        /// <summary>
        /// Method to get one .exe file from every directory
        /// that match to the pattern
        /// </summary>
        /// <param name="directory">Directory from which get directories</param>
        /// <param name="toAdd">list to which add searched file</param>
        /// <param name="FolderPattern">In which folders to look for</param>
        private void GetExeFromDirectory(string directory, List<string> toAdd, string FolderPattern, CancellationToken token)
        {
            try
            {
                //If cancel task is requested, abort doing task
                if(token.IsCancellationRequested)
                {
                    return;
                }

                //Get all subDirectories in the directory
                string[] subdirectoryEntries = Directory.GetDirectories(directory);

                foreach (string subdirectory in subdirectoryEntries)
                {
                    //If directory match to the pattern
                    if (subdirectory.ToUpper().Contains(FolderPattern))
                    {
                        //Get all first directories in this directory
                        var FoldersWithGamesInGivenDirectory = Directory.GetDirectories(subdirectory);

                        //and for each directory, that match to the pattern
                        //add .exe file from this directory
                        foreach (var filePath in FoldersWithGamesInGivenDirectory)
                        {
                            toAdd.Add(StringHelpers.ClosestExeToFileName(filePath));
                        }

                        return;
                    }

                    GetExeFromDirectory(subdirectory, toAdd, FolderPattern, token);
                    
                }
            }
            catch (UnauthorizedAccessException)
            { //Skip
            }
        }
        #endregion
    }
}
