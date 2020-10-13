﻿using Microsoft.Win32;
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
        /// Image in bytes
        /// </summary>
        public byte[] ImageBytes { get; set; } = ViewModelApplication.ApplicationUser.Image;

        /// <summary>
        /// Image as BitmapImage
        /// </summary>
        public BitmapImage UserImage => ViewModelApplication.ApplicationUser.UserImage;

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
            LogoutCommand = new RelayCommand(async() => await Logout());
            UploadNewPhotoCommand = new RelayCommand(async() => await UploadNewPhotoAsync());

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
            await RunCommandAsync(() => SaveChangesIsRunning, async() => 
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

                    if(context != null)
                    {
                        _logger.Info($"User with ID: {ViewModelApplication.ApplicationUser.Id} changed Username: {ViewModelApplication.ApplicationUser.Username} to {Name} successfully");

                        //Set header name to new username
                        HeaderName = Name;

                        //Set ApplicationUser Username to new username
                        ViewModelApplication.ApplicationUser.Username = Name;

                        //Get new JWTToken
                        ViewModelApplication.ApplicationUser.JWTToken = context.JWTToken;

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

                    if(context != null)
                    {
                        _logger.Info($"User with ID: {ViewModelApplication.ApplicationUser.Email} changed Email: {ViewModelApplication.ApplicationUser.Email} to {Email} successfully");

                        //Set ApplicationUser Email to new Email
                        ViewModelApplication.ApplicationUser.Email = Email;

                        //Get new JWTToken
                        ViewModelApplication.ApplicationUser.JWTToken = context.JWTToken;

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
                            ViewModelApplication.ApplicationUser.JWTToken = context.JWTToken;

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
            await ViewModelApplication.Logout();
        }

        /// <summary>
        /// Method that handle uploading new photo
        /// </summary>
        private async Task UploadNewPhotoAsync()
        {
            await RunCommandAsync(() => UploadNewPhotoIsRunning, async () =>
            {
                _logger.Debug("User attempt to change image");
                //Create new OpenFileDialog
                var Dialog = new OpenFileDialog();

                //Open new dialog, await to choose a file, or close window
                Dialog.ShowDialog();

                //Set pathToImage to the given path...
                var pathToImage = Dialog.FileName.ToUpper();

                //If path is not empty or null...
                if ((!(string.IsNullOrEmpty(pathToImage))))
                {
                    //Create new Model, which will be sent to API
                    var ImageToChange = new ChangeImageRequest();

                    if (!pathToImage.EndsWith(".JPG") && !pathToImage.EndsWith(".JPE") && !pathToImage.EndsWith(".BMP") && !pathToImage.EndsWith(".GIF") && !pathToImage.EndsWith(".PNG"))
                    {
                        PopupExtensions.NewInfoPopup("Wrong format of photo. Acceptable extensions: .JPG, .JPE, .BMP, .GIF, .PNG");
                        return;
                    }
                    
                    try
                    {
                        //Take a image from PC
                        Image imageFromPC = Image.FromFile(pathToImage);

                        //Set Image in model, as byte[] image
                        ImageToChange.Image = imageFromPC.ImageToBytes();
                    }
                    catch(Exception ex)
                    {
                        PopupExtensions.NewErrorPopup("Wrong format of photo. Acceptable extensions: .JPG, .JPE, .BMP, .GIF, .PNG");
                        _logger.Warn(ex, "User tried to upload wrong image format");
                    }

                    //Send request to the API
                    var result = await HttpExtensions.HandleHttpRequestAsync(UserRoutes.ChangeImage, ImageToChange);
                    
                    //If ApiResponse is successful
                    if (result)
                    {
                        //Set current image of user, to new image
                        ImageBytes = ImageToChange.Image;

                        //Set ApplicationUser Image, to new image
                        ViewModelApplication.ApplicationUser.Image = ImageToChange.Image;
                        _logger.Info($"User with ID: {ViewModelApplication.ApplicationUser.Id}");
                    }
                }
                else
                {
                    _logger.Debug("Did not changed Image.No new image was selected.");
                }
            });
        }

        #endregion
    }
}
