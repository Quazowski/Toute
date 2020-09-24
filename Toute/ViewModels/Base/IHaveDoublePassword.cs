using System.Security;

namespace Toute
{
    /// <summary>
    /// Interface that helps handling sending two SecurePasswords to ViewModel
    /// </summary>
    public interface IHaveDoublePassword
    {
        /// <summary>
        /// SecureString that will be sent to ViewModel
        /// </summary>
        public SecureString FirstSecureString { get; }

        /// <summary>
        /// SecureString that will be sent to ViewModel
        /// </summary>
        public SecureString SecondSecureString { get; }
    }
}
