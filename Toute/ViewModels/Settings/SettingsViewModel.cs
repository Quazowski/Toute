using Microsoft.Win32;
using NLog;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Toute.Core;
using Toute.Core.Routes;
using Toute.Extensions;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// ViewModel for <see cref="SettingsPage"/>
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        #region Private members

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public members

        /// <summary>
        /// Name of user, that is displayed on the top 
        /// </summary>
        //public string HeaderName { get; set; } = ViewModelApplication.ApplicationUser.Username;
        public string HeaderName { get; set; } = ViewModelApplication.ApplicationUser.Username;

        /// <summary>
        /// Name of user, that can be edited
        /// </summary>
        public string Name { get; set; } = ViewModelApplication.ApplicationUser.Username;

        /// <summary>
        /// Email of user, that can be edited
        /// </summary>
        public string Email { get; set; } = ViewModelApplication.ApplicationUser.Email;

        /// <summary>
        /// Image as BitmapImage
        /// </summary>
        public BitmapImage UserImage { get; set; } = ViewModelApplication.ApplicationUser.UserImage;

        /// <summary>
        /// Status of <see cref="SaveChangesAsync(object)"/>
        /// </summary>
        public bool SaveChangesIsRunning { get; set; }

        /// <summary>
        /// Status of <see cref="UploadNewPhotoAsync(object)"/>
        /// </summary>
        public bool UploadNewPhotoIsRunning { get; set; }

        #endregion

        #region Public commands

        /// <summary>
        /// Command that is fired, when user click Save Changes button
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// Command that handle Logout from Application
        /// </summary>
        public ICommand LogoutCommand { get; set; }

        /// <summary>
        /// Command that handle upload of new photo
        /// </summary>
        public ICommand UploadNewPhotoCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsViewModel()
        {
            _logger.Info("Start setting up SettingsViewModel");

            //Create commands
            SaveChangesCommand = new ParametrizedRelayCommand(async (credentials) => await SaveChangesAsync(credentials));
            LogoutCommand = new RelayCommand(async () => await Logout());
            UploadNewPhotoCommand = new RelayCommand(async () => await UploadNewPhotoAsync());

            _logger.Info("Done setting up SettingsViewModel");
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Method that save new user Credentials
        /// </summary>
        /// <param name="credentials">New Credentials</param>
        private async Task SaveChangesAsync(object credentials)
        {
            await RunCommandAsync(() => SaveChangesIsRunning, async () =>
            {
                //If username is changed...
                if (Name != ViewModelApplication.ApplicationUser.Username)
                {
                    _logger.Debug($"Trying to change username from {ViewModelApplication.ApplicationUser.Username} to {Name}");

                    //Send request to the API
                    var context = await HttpExtensions.HandleHttpRequestOfTResponseAsync<CredentialChangedResponse>(UserRoutes.ChangeUsername, new ChangeUsernameRequest
                    {
                        NewUsername = Name
                    });

                    if (context != null)
                    {
                        _logger.Info($"User with ID: {ViewModelApplication.ApplicationUser.Id} changed Username: {ViewModelApplication.ApplicationUser.Username} to {Name} successfully");

                        //Set header name to new username
                        HeaderName = Name;

                        //Set ApplicationUser Username to new username
                        ViewModelApplication.ApplicationUser.Username = Name;

                        //Get new JWTToken
                        ViewModelApplication.ApplicationUser.Token = context.Token;

                        //Show message
                        PopupExtensions.NewInfoPopup($"Name changed successfully.");
                    }
                    else
                    {
                        _logger.Debug($"Failed to change username from {ViewModelApplication.ApplicationUser.Username} to {Name}");
                    }
                }

                //If Email is changed...
                if (Email != ViewModelApplication.ApplicationUser.Email)
                {
                    _logger.Debug($"Trying to change user email from {ViewModelApplication.ApplicationUser.Email} to {Email}");

                    //Send request to the API
                    var context = await HttpExtensions.HandleHttpRequestOfTResponseAsync<CredentialChangedResponse>(UserRoutes.ChangeEmail, new ChangeEmailRequest
                    {
                        NewEmail = Email
                    });

                    if (context != null)
                    {
                        _logger.Info($"User with ID: {ViewModelApplication.ApplicationUser.Email} changed Email: {ViewModelApplication.ApplicationUser.Email} to {Email} successfully");

                        //Set ApplicationUser Email to new Email
                        ViewModelApplication.ApplicationUser.Email = Email;

                        //Get new JWTToken
                        ViewModelApplication.ApplicationUser.Token = context.Token;

                        //Show message
                        PopupExtensions.NewInfoPopup("Email changed successfully.");
                    }
                    else
                    {
                        _logger.Debug($"Failed to change user email from {ViewModelApplication.ApplicationUser.Email} to {Email}");
                    }

                }

                //If there is new password sent...
                if (!(string.IsNullOrEmpty((credentials as SettingsPage).CurrentPassword.SecurePassword.Unsecure())))
                {
                    _logger.Debug($"Trying to change password");
                    //And password and confirm password match...
                    if ((credentials as SettingsPage).Password.SecurePassword.Unsecure() == (credentials as SettingsPage).ConfirmPassword.SecurePassword.Unsecure())
                    {
                        //Send request to the API
                        var context = await HttpExtensions.HandleHttpRequestOfTResponseAsync<CredentialChangedResponse>(UserRoutes.ChangePassword, new ChangePasswordRequest
                        {
                            CurrentPassword = (credentials as SettingsPage).CurrentPassword.SecurePassword.Unsecure(),
                            NewPassword = (credentials as SettingsPage).Password.SecurePassword.Unsecure()
                        });

                        //If ApiResponse is successful
                        if (context != null)
                        {
                            //Get new JWTToken
                            ViewModelApplication.ApplicationUser.Token = context.Token;

                            //Show message
                            PopupExtensions.NewInfoPopup("Password changed successfully.");

                            _logger.Info($"User with ID: {ViewModelApplication.ApplicationUser.Email} successfully changed password");
                        }
                        else
                        {
                            _logger.Debug($"Failed to change password.");
                        }
                    }
                    else
                    {
                        _logger.Debug($"Failed to change password. New password, and confirm new password do not match");
                    }
                }
            });
        }

        /// <summary>
        /// Method that logout user from Application
        /// </summary>
        public async Task Logout()
        {
            await ViewModelApplication.LogoutAsync();
        }

        /// <summary>
        /// Method that handle uploading new photo
        /// </summary>
        private async Task UploadNewPhotoAsync()
        {
            await RunCommandAsync(() => UploadNewPhotoIsRunning, async () =>
            {

                var imageToChange = ImageExtension.GetImageFromPCinBytes();

                if(imageToChange != null)
                {
                    //Send request to the API
                    var result = await HttpExtensions.HandleHttpRequestAsync(UserRoutes.ChangeImage, new ChangeImageRequest
                    {
                        Image = imageToChange
                    });

                    //If ApiResponse is successful
                    if (result)
                    {
                        //Set ApplicationUser Image, to new image
                        ViewModelApplication.ApplicationUser.Image = imageToChange;

                        //Set new image in settings page
                        UserImage = imageToChange.BytesToBitMapImage();

                        _logger.Info($"User with [ID]: [{ViewModelApplication.ApplicationUser.Id}] changed photo");
                    }
                }
            });
        }

        #endregion
    }
}
