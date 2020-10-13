namespace Toute
{
    /// <summary>
    /// Design model for information and error popup
    /// </summary>
    public class InfoControlDesignModel : InfoControlViewModel
    {
        /// <summary>
        /// Static instance of <see cref="InfoControlDesignModel"/>
        /// </summary>
        public static InfoControlDesignModel Instance = new InfoControlDesignModel();

        /// <summary>
        /// Default constructor
        /// </summary>
        public InfoControlDesignModel()
        {
            Message = "Design time information";
            IsError = true;
        }
    }
}
