using System.Security;

namespace Toute
{
    /// <summary>
    /// Interface that helps handling sending SecurePassword to ViewModel
    /// </summary>
    public interface IHavePassword
    {
        /// <summary>
        /// SecureString that will be sent to ViewModel
        /// </summary>
        public SecureString SecureString { get; }

    }
}
