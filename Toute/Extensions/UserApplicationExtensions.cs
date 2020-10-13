﻿using NLog;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Toute.Core;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// Helpers for managing User in application
    /// </summary>
    public static class UserApplicationHelpers
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Helper that help user get all user credentials from local Db
        /// and then user them to login to application
        /// </summary>
        /// <param name="_logger">Logger</param>
        /// <returns></returns>
        public static async Task LoginToApp()
        {
            _logger.Info("Attempt to login to application from local DB");

            //Get user credentials from LocalDB
            var credentials = await SqliteDb.GetLoginCredentialsAsync();

            //Make a list of friends
            var Friends = new ObservableCollection<FriendModel>();

            //Make a ApplicationUserModel
            var user = new ApplicationUserModel
            {
                Id = credentials.Id,
                Username = credentials.Username,
                Email = credentials.Email,
                Friends = Friends,
                Image = credentials.Image,
                JWTToken = credentials.JWTToken
            };

            //Set application user to made user
            ViewModelApplication.ApplicationUser = user;

            _logger.Debug($"User of Username: {user.Username} and ID: {user.Id} logged to application");


            _logger.Trace($"Trying to get all files from local DB");
            //Loads all files that were added
            var items = SqliteDb.GetGames(ViewModelApplication.ApplicationUser?.Id).Result;

            //For every file, that were added.... 
            foreach (var file in items)
            {
                //Add to Item list a GameModel
                ViewModelGame.Items.Add(new GameModel
                {
                    Title = file.Title,
                    FileId = file.Id,
                    Path = file.Path,
                    BytesImage = file.Image
                });
            }

            _logger.Trace($"Got all files from local DB");

            _logger.Info("Refreshing friends of the user is running...");

            //Run refreshing friend list
            ViewModelApplication.ApplicationUser.RefreshFriends = new Timer(async (e) =>
            {
                await ViewModelSideMenu.RefreshFriendsAsync(ViewModelApplication.Friends);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));


        }
    }
}
