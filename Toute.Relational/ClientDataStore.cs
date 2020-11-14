using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

            //Add user to local DB
            DbContext.LoginCredentials.Add(loginCredentials);

            //Save local DB
            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Remove user credentials from DB
        /// </summary>
        /// <returns></returns>
        public async Task RemoveLoginCredentialsAsync()
        {
            //remove all user credentials
            DbContext.LoginCredentials.RemoveRange(DbContext.LoginCredentials);

            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Adds game to the DB for a specific user
        /// </summary>
        /// <param name="game">Game/Files to add</param>
        /// <returns>Task when finish</returns>
        public async Task AddGameAsync(GameDataModel game)
        {
            if (game == null)
                return;

            if (game.UserId == null)
            {
                //Version of UserId for users that are not logged.
                game.UserId = "4DDBB5B5-B4F0-4D09-AE72-35594591FF4A";
                DbContext.Game.Add(game);
            }
            else
            {
                DbContext.Game.Add(game);
            }

            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Removes game from DB
        /// </summary>
        /// <param name="Id">ID of the user</param>
        /// <returns>Task when finish</returns>
        public async Task RemoveGameAsync(string Id)
        {
            //Find game with given ID
            var game = DbContext.Game.Include(x => x.Paths).FirstOrDefault(x => x.Id == Id);
            if (game == null)
                return;

            if (game.UserId == null)
            {
                //Version of UserId for users that are not logged.
                game.UserId = "4DDBB5B5-B4F0-4D09-AE72-35594591FF4A";
                DbContext.Game.Remove(game);
            }
            else
            {
                DbContext.Game.Remove(game);
            }

            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Change value of game
        /// </summary>
        /// <param name="model">GameDataModel that will be applied on current game</param>
        /// <returns>Task when finish</returns>
        public async Task ChangeValuesAsync(GameDataModel model)
        {
            //Find game
            var file = DbContext.Game.FirstOrDefault(x => x.Id == model.Id);

            if (file == null)
                return;

            if (model.UserId == null)
            {
                //Version of UserId for users that are not logged.
                model.UserId = "4DDBB5B5-B4F0-4D09-AE72-35594591FF4A";
            }

            //If there is new image...
            if (model.Image != null)
                file.Image = model.Image;

            //If there is new path...
            if (model.Paths != null)
                file.Paths = model.Paths;

            //If there is new title...
            if (model.Title != null)
                file.Title = model.Title;


            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get all games from DB that are saved for specific user
        /// </summary>
        /// <param name="UserId">User that added games</param>
        /// <returns>List of games as task result</returns>
        public Task<List<GameDataModel>> GetGamesAsync(string UserId)
        {
            //Make a list, and add to this list all games, that were added by the user
            var games = new List<GameDataModel>();

            if (string.IsNullOrEmpty(UserId))
            {
                //Version for user, that are not logged
                games = DbContext.Game.Include(x => x.Paths).Where(x => x.UserId == "4DDBB5B5-B4F0-4D09-AE72-35594591FF4A")?.ToList();
            }
            else
            {
                games = DbContext.Game.Include(x => x.Paths).Where(x => x.UserId == UserId)?.ToList();
            }

            return Task.FromResult(games);
        }

        /// <summary>
        /// Change user JWTToken and refresh token
        /// </summary>
        /// <param name="Token">JWTToken</param>
        /// <param name="RefreshToken">RefreshToken</param>
        /// <returns>Task when finish</returns>
        public async Task ChangeUserTokensAsync(string Token, string RefreshToken)
        {
            //Find user
            var user = DbContext.LoginCredentials.FirstOrDefault();

            //Set tokens for the user
            user.RefreshToken = RefreshToken;
            user.Token = Token;

            await DbContext.SaveChangesAsync();
        }

        public async Task ChangeUserUsernameAsync(string newUsername, string newToken)
        {
            //Find user
            var user = DbContext.LoginCredentials.FirstOrDefault();

            //Set token and username for a user
            user.Username = newUsername;
            user.Token = newToken;

            await DbContext.SaveChangesAsync();
        }

        public async Task ChangeUserEmailAsync(string newEmail, string newToken)
        {
            //Find user
            var user = DbContext.LoginCredentials.FirstOrDefault();

            //Set tokens for the user
            user.Email = newEmail;
            user.Token = newToken;

            await DbContext.SaveChangesAsync();
        }

        #endregion

    }
}
