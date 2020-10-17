using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    public class RefreshTokenRequest
    {
        /// <summary>
        /// Authorization token
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// Refresh token
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }
    }
}
