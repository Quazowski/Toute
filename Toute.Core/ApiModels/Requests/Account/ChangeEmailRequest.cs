using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    /// <summary>
    /// Request to change email
    /// </summary>
    public class ChangeEmailRequest
    {
        /// <summary>
        /// Email of the user
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Email is not valid")]
        public string NewEmail { get; set; }
    }
}
