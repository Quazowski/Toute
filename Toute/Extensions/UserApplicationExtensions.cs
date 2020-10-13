using Microsoft.Extensions.Logging;
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
        /// <summary>
        /// Helper that help user get all user credentials from local Db
        /// and then user them to login to application
        /// </summary>
        /// <param name="_logger">Logger</param>
        /// <returns></returns>
        public static async Task LoginToApp(ILogger<App> _logger)
        {
            _logger.LogInformation("Attempt to login to application from local DB");

            //Get user credentials from LocalDB
            var credentials = await SqliteDb.GetLoginCredentialsAsync();

            //Make a list of friends
            var Friends = new ObservableCollection<FriendModel>();

            foreach (var friend in credentials.Friends)
            {
                var messages = new ObservableCollection<MessageModel>();
                foreach (var message in friend.Messages)
                {
                    messages.Add(new MessageModel
                    {
                        Message = message.Message,
                        DateOfSent = message.DateOfSent,
                        SentByMe = message.SentByMe
                    });
                }
                Friends.Add(new FriendModel
                {
                    FriendId = friend.FriendId,
                    Status = friend.Status,
                    Messages = messages
                });
            }
            var user = new ApplicationUserModel
            {
                Id = credentials.Id,
                Username = credentials.Username,
                Email = credentials.Email,
                Friends = Friends,
                Image = credentials.Image,
                JWTToken = credentials.JWTToken
            };

            ViewModelApplication.ApplicationUser = user;

            _logger.LogDebug($"User of Username: -{user.Username}- and ID: -{user.Id}- logged to application");

            _logger.LogInformation("Refreshing friends of the user is running...");
            ViewModelApplication.ApplicationUser.RefreshFriends = new Timer(async (e) =>
            {
                await ViewModelSideMenu.RefreshFriendsAsync(ViewModelApplication.Friends);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));


        }
    }
}
