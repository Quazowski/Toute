using System.Threading.Tasks;

namespace Toute.Core
{
    /// <summary>
    /// Stores information about the client application
    /// </summary>
    public interface IClientDataStore
    {
        /// <summary>
        /// Determines if user is logged
        /// </summary>
        bool HasCredentials();

        /// <summary>
        /// Makes sure the client data store is set up
        /// </summary>
        /// <returns>Task once setup is completed</returns>
        Task EnsureDataStoreAsync();

        /// <summary>
        /// Get the stored credentials for this client
        /// </summary>
        /// <returns>Return credentials if the exists</returns>
        Task<LoginCredentialsDataModel> GetLoginCredentialsAsync();

        /// <summary>
        /// Save login credentials
        /// </summary>
        /// <param name="loginCredentials"user credentials></param>
        /// <returns>Return task when task is completed</returns>
        Task SaveLoginCredentialsAsync(LoginCredentialsDataModel loginCredentials);

        /// <summary>
        /// Remove login credentials
        /// </summary>
        /// <param name="Id"User Id></param>
        /// <returns>Return task when task is completed</returns>
        Task RemoveLoginCredentialsAsync();
    }
}
