using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
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
        private ObservableCollection<GameModel> _items;
        public ObservableCollection<GameModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(_items));
            }
        }

        public bool PopupHidden { get; set; } = true;

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

        /// <summary>
        /// Default Constructor for GamesPageViewModel
        /// </summary>
        public GamesPageViewModel()
        {
            //Creating new ObservableCollection to store Games
            Items = new ObservableCollection<GameModel>();

            Items.CollectionChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(Items));
            };

            //Command to run chosen game
            RunCommand = new ParametrizedRelayCommand((path) => RunGame((string)path));

            //Command to open settings of chosen game
            SettingsCommand = new ParametrizedRelayCommand((path) => OpenSettingsGame((string)path));

            //Command that delete game with given path
            RemoveGameCommand = new RelayCommand(DeleteGame);

            //Command that add game
            AddGameCommand = new RelayCommand(AddGame);

            //Load from local machine db and set items
            Items.Add(new GameModel { Title = "Config Folder", PathToGame = @"C:\Users\wardl\Desktop\config" });
            Items.Add(new GameModel { Title = "22", PathToGame = "D..." });
            Items.Add(new GameModel { Title = "22", PathToGame = "D..." });
            Items.Add(new GameModel { Title = "22", PathToGame = "D..." });
            Items.Add(new GameModel { Title = "22", PathToGame = "D..." });
            Items.Add(new GameModel { Title = "22", PathToGame = "D..." });
        }

        private void AddGame()
        {
            AddGameWindow gameWindow = new AddGameWindow();
            gameWindow.Show();
            //Open window, select item, take of this icon and path, add to Items.
        }

        private void DeleteGame()
        {
            CurrentItem.PopupOpen = false;

            Items.Remove(CurrentItem);

            OnPropertyChanged(nameof(Items));
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
            //Toggle popup
            CurrentItem = Items.FirstOrDefault(x => x.PathToGame == path);

            CurrentItem.PopupOpen ^= true;


        }
    }
}
