using System.Collections.ObjectModel;

namespace Toute
{
    /// <summary>
    /// A design Model that display list of GamesModels
    /// in design time
    /// </summary>
    public class GameModelListItemDesignModel : GamesViewModel
    {
        /// <summary>
        /// Make a static instance of this class
        /// </summary>
        public static GameModelListItemDesignModel Instance = new GameModelListItemDesignModel();

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameModelListItemDesignModel()
        {
            //Sets items
            Items = new ObservableCollection<GameModel>
            {
                new GameModel
                {
                    Title = "Name in Design Time",
                },
                new GameModel
                {
                    Title = "Second Name in Design Time",
                },
                new GameModel
                {
                    Title = "Third Name in Design Time",
                },
            };
        }
    }
}


