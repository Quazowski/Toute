using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Toute
{
    public class GameModelListItemDesignModel : GamesPageViewModel
    {
        public static GameModelListItemDesignModel Instance = new GameModelListItemDesignModel();

        public GameModelListItemDesignModel()
        {
            Items = new ObservableCollection<GameModel>
            {
                new GameModel
                {
                    Title = "Name in Design Time",
                    PathToGame = "Path in Design Time"
                },
                new GameModel
                {
                    Title = "Second Name in Design Time",
                    PathToGame = "Second Path in Design Time"
                },
                new GameModel
                {
                    Title = "Third Name in Design Time",
                    PathToGame = "Third Path in Design Time"
                },
            };
        }
    }
}


