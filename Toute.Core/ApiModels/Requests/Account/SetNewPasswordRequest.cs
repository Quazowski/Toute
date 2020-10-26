using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    public class SetNewPasswordRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string PasswordToken { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Password too short")]
        public string Password { get; set; }
    }
}
