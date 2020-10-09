using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    /// <summary>
    /// Request to register
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Username of the user
        /// </summary>
        [Required(ErrorMessage = "Username is required!")]
        [MinLength(3, ErrorMessage = "Username too short!")]
        [MaxLength(22, ErrorMessage = "Username too long!")]
        [RegularExpression(@"^[A-Za-z0-9]{3,22}$", ErrorMessage = "Username can not have whitespace, comma, or special character")]
        public string Username { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"+ "@"+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        /// <summary>
        /// Password of the user
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Password too short")]
        [MaxLength(255, ErrorMessage = "Password too long")]
        public string Password { get; set; }
    }
}
