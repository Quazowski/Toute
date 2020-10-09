
using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    /// <summary>
    /// Request to change a image
    /// </summary>
    public class ChangeImageRequest
    {
        /// <summary>
        /// Image on which be changed
        /// </summary>
        [Required(ErrorMessage = "Current Password is required")]
        public byte[] Image { get; set; }
    }
}
