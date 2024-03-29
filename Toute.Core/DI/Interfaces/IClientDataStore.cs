﻿using System.Collections.Generic;
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

        /// <summary>
        /// Adds game to the list of games
        /// </summary>
        /// <returns></returns>
        Task<List<GameDataModel>> GetGamesAsync(string UserId);

        /// <summary>
        /// Adds game to the list of games
        /// </summary>
        /// <returns></returns>
        Task AddGameAsync(GameDataModel game);

        /// <summary>
        /// Remove game from the list of games
        /// </summary>
        /// <returns></returns>
        Task RemoveGameAsync(string Id);

        /// <summary>
        /// Change value of the file
        /// </summary>
        /// <returns></returns>
        Task ChangeValuesAsync(GameDataModel model);

        /// <summary>
        /// Change user token and refresh token
        /// </summary>
        /// <param name="Token">JWToken</param>
        /// <param name="RefreshToken">Refresh token</param>
        /// <returns></returns>
        Task ChangeUserTokensAsync(string Token, string RefreshToken);

        /// <summary>
        /// Change user username and token
        /// </summary>
        /// <param name="newUsername">New username for a user</param>
        /// <param name="newToken">New token</param>
        /// <returns></returns>
        Task ChangeUserUsernameAsync(string newUsername, string newToken);

        /// <summary>
        /// Change user email and token
        /// </summary>
        /// <param name="newUsername">New username for a user</param>
        /// <param name="newToken">New token</param>
        /// <returns></returns>
        Task ChangeUserEmailAsync(string newEmail, string newToken);
    }
}
