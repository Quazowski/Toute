using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    /// <summary>
    /// Request to login
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Username of the user
        /// </summary>
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        /// <summary>
        /// Password of the user
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
