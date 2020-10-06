namespace Toute.Core
{
    /// <summary>
    /// Response to any credentials change
    /// </summary>
    public class CredentialChangedResponse
    {
        /// <summary>
        /// Authorization token
        /// </summary>
        public string JWTToken { get; set; }
    }
}
