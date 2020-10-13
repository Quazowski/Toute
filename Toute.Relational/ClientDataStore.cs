using System;
using System.Linq;
using System.Threading.Tasks;
using Toute.Core;

namespace Toute.Relational
{
    /// <summary>
    /// Stores information about the client application
    /// </summary>
    public class ClientDataStore : IClientDataStore
    {
        #region Protected Members

        /// <summary>
        /// The database for client data store
        /// </summary>
        protected ClientDataStoreDbContext DbContext;

        #endregion

        #region Public Members

        /// <summary>
        /// Determines if user is logged
        /// </summary>
        public bool HasCredentials()
        {
            return DbContext.LoginCredentials.Any();
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public ClientDataStore(ClientDataStoreDbContext dbContext)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Interface implementations

        /// <summary>
        /// Makes sure the client data store is set up
        /// </summary>
        /// <returns>Task once setup is completed</returns>
        public async Task EnsureDataStoreAsync()
        {
            await DbContext.Database.EnsureCreatedAsync();
        }

        /// <summary>
        /// Get the stored credentials for this client
        /// </summary>
        /// <returns>Return credentials if the exists</returns>
        public Task<LoginCredentialsDataModel> GetLoginCredentialsAsync()
        {
            return Task.FromResult(DbContext.LoginCredentials.FirstOrDefault());
        }

        /// <summary>
        /// Save login credentials
        /// </summary>
        /// <param name="loginCredentials"></param>
        /// <returns>Return task when task is completed</returns>
        public async Task SaveLoginCredentialsAsync(LoginCredentialsDataModel loginCredentials)
        {
            //Clear first
            DbContext.LoginCredentials.RemoveRange(DbContext.LoginCredentials);

            //Add
            DbContext.LoginCredentials.Add(loginCredentials);

            //
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveLoginCredentialsAsync()
        {
            DbContext.LoginCredentials.RemoveRange(DbContext.LoginCredentials);

            await DbContext.SaveChangesAsync();
        }

        #endregion

    }
}
