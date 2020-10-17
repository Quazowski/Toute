using System;

namespace Toute.Core
{
    /// <summary>
    /// Model of message that is saved in DB
    /// </summary>
    public class GameDataModel
    {
        /// <summary>
        /// Unique Id for the game
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Id of the user that owns path to the game
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Title of the file
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Path to game
        /// </summary>
        public virtual string Path { get; set; }

        /// <summary>
        /// Image in byte[]
        /// </summary>
        public virtual byte[] Image{ get; set; }
    }
}
