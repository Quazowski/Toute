using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    /// <summary>
    /// Request to change password
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Current password
        /// </summary>
        [Required(ErrorMessage = "Current Password is required")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// New password of the user
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Password too short")]
        [MaxLength(255, ErrorMessage = "Password too long")]
        public string NewPassword { get; set; }
    }
}
