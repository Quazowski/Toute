using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Toute.Core;
using Toute.Core.Routes;
using Toute.Extensions;

namespace Toute
{
    /// <summary>
    /// ViewModel for <see cref="SettingsPage"/>
    /// </summary>
    public class SettingsPageViewModel : BaseViewModel
    {
        #region Public members

        /// <summary>
        /// Name of user, that is displayed on the top 
        /// </summary>
        public string HeaderName { get; set; } = IoC.Get<ApplicationViewModel>().ApplicationUser.Username;

        /// <summary>
        /// Name of user, that can be edited
        /// </summary>
        public string Name { get; set; } = IoC.Get<ApplicationViewModel>().ApplicationUser.Username;

        /// <summary>
        /// Email of user, that can be edited
        /// </summary>
        public string Email { get; set; } = IoC.Get<ApplicationViewModel>().ApplicationUser.Email;

        /// <summary>
        /// Image in bytes
        /// </summary>
        public byte[] ImageBytes { get; set; } = IoC.Get<ApplicationViewModel>().ApplicationUser.Image;

        /// <summary>
        /// Image as BitmapImage
        /// </summary>
        public BitmapImage UserImage => IoC.Get<ApplicationViewModel>().ApplicationUser.UserImage;

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
        public SettingsPageViewModel()
        {
            //Create commands
            SaveChangesCommand = new ParametrizedRelayCommand(async (credentials) => await SaveChangesAsync(credentials));
            LogoutCommand = new RelayCommand(Logout);
            UploadNewPhotoCommand = new RelayCommand(async() => await UploadNewPhotoAsync());
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
                if (Name != IoC.Get<ApplicationViewModel>().ApplicationUser.Username)
                {
                    //If name length is less that four...
                    if (Name.Length < 4)
                    {
                        //Add error to Final message
                        PopupExtensions.NewInfoPopup($"Name not changed, reason: Name must contains at least 5 letters. ");
                    }
                    //Otherwise...
                    else
                    {
                        //Send request to the API
                        var context = await HttpExtensions.HandleHttpRequestOfTResponseAsync<CredentialChangedResponse>(UserRoutes.ChangeUsername, new ChangeUsernameRequest
                        {
                            NewUsername = Name
                        });

                        if(context != null)
                        {
                            //Set header name to new username
                            HeaderName = Name;

                            //Set ApplicationUser Username to new username
                            IoC.Get<ApplicationViewModel>().ApplicationUser.Username = Name;

                            //Get new JWTToken
                            IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken = context.JWTToken;

                            //Show message
                            PopupExtensions.NewInfoPopup($"Name changed successfully. ");
                        }
                    }
                }

                //If Email is changed...
                if (Email != IoC.Get<ApplicationViewModel>().ApplicationUser.Email)
                {
                    //If Email length is less that four...
                    if (Email.Length < 4)
                    {
                        //Add error to Final message
                        PopupExtensions.NewInfoPopup($"Email must contains at least 5 letters!");
                    }
                    //If email does not have @ letter
                    else if (!(Email.Contains('@')))
                    {
                        //Show message
                        PopupExtensions.NewInfoPopup($"Email is not valid!");
                    }
                    //Otherwise...
                    else
                    {
                        //Send request to the API
                        var context = await HttpExtensions.HandleHttpRequestOfTResponseAsync<CredentialChangedResponse>(UserRoutes.ChangeEmail, new ChangeEmailRequest
                        {
                            NewEmail = Email
                        });

                        if(context != null)
                        {
                            //Set ApplicationUser Email to new Email
                            IoC.Get<ApplicationViewModel>().ApplicationUser.Email = Email;

                            //Get new JWTToken
                            IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken = context.JWTToken;

                            //Show message
                            PopupExtensions.NewInfoPopup("Email changed successfully.");
                        }
                    }
                }

                //If there is new password sent...
                if (!(string.IsNullOrEmpty((credentials as SettingsPage).CurrentPassword.SecurePassword.Unsecure())))
                {
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
                            IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken = context.JWTToken;

                            //Show message
                            PopupExtensions.NewInfoPopup("Password changed successfully. ");
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Method that logout user from Application
        /// </summary>
        public void Logout()
        {
            IoC.Get<ApplicationViewModel>().Logout();
        }

        /// <summary>
        /// Method that handle uploading new photo
        /// </summary>
        private async Task UploadNewPhotoAsync()
        {
            await RunCommandAsync(() => UploadNewPhotoIsRunning, async () =>
            {
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
                    catch(Exception)
                    {
                        PopupExtensions.NewInfoPopup("Wrong format of photo. Acceptable extensions: .JPG, .JPE, .BMP, .GIF, .PNG");
                    }

                    //Send request to the API
                    var result = await HttpExtensions.HandleHttpRequestAsync(UserRoutes.ChangeImage, ImageToChange);
                    
                    //If ApiResponse is successful
                    if (result)
                    {
                        //Set current image of user, to new image
                        ImageBytes = ImageToChange.Image;

                        //Set ApplicationUser Image, to new image
                        IoC.Get<ApplicationViewModel>().ApplicationUser.Image = ImageToChange.Image;
                    }
                }
            });
        }

        #endregion
    }
}
