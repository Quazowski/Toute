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
        public string Username { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password of the user
        /// </summary>
        public string Password { get; set; }
    }
}
