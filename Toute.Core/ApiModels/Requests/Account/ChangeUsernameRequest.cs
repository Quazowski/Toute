using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    /// <summary>
    /// Request to change user username
    /// </summary>
    public class ChangeUsernameRequest
    {
        /// <summary>
        /// Username of the user
        /// </summary>
        [Required(ErrorMessage = "Username is required!")]
        [MinLength(3, ErrorMessage = "Username too short!")]
        [MaxLength(22, ErrorMessage = "Username too long!")]
        [RegularExpression(@"^[A-Za-z0-9]{3,22}$", ErrorMessage = "Username can not have whitespace, comma, or special character")]
        public string NewUsername { get; set; }
    }
}
