namespace Toute
{
    /// <summary>
    /// A design model for displaying single item of GameModel
    /// in design time
    /// </summary>
    public class GameModelItemDesignModel : GameModel
    {
        /// <summary>
        /// Make a static instance of this class
        /// </summary>
        public static GameModelItemDesignModel Instance = new GameModelItemDesignModel();

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameModelItemDesignModel()
        {
            //Sets title
            Title = "Name of game Design Time";
            //Sets PathToGame
            PathToGame = "Path of game Design Time";
        }
    }
}
