namespace Toute
{
    public class GameModelItemDesignModel : GameModel
    {
        public static GameModelItemDesignModel Instance = new GameModelItemDesignModel();

        public GameModelItemDesignModel()
        {
            Title = "Name of game Design Time";
            PathToGame = "Path of game Design Time";
        }
    }
}
