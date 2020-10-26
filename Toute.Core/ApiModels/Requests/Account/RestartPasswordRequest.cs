using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    /// <summary>
    /// Request to change email
    /// </summary>
    public class RestartPasswordRequest
    {
        /// <summary>
        /// Email of the user
        /// </summary>
        [Required(ErrorMessage = "Username or email is required")]
        public string UsernameOrEmail { get; set; }
    }
}
