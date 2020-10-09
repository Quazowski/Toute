using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    /// <summary>
    /// Model, that contains basic information
    /// about the friend
    /// </summary>
    public class RelationshipModel
    {
        /// <summary>
        /// ID of friend
        /// </summary>
        [Required(ErrorMessage = "You must provide a ID!")]
        public string FriendId { get; set; }
    }
}
