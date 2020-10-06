using System.Collections.ObjectModel;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text;
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
        public BitmapImage UserImage => ImageBytes?.BytesToBitMapImage();

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
            SaveChangesCommand = new ParametrizedRelayCommand((credentials) => SaveChanges(credentials));
            LogoutCommand = new RelayCommand(Logout);
            UploadNewPhotoCommand = new RelayCommand(UploadNewPhoto);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Method that save new user Credentials
        /// </summary>
        /// <param name="credentials">New Credentials</param>
        private async void SaveChanges(object credentials)
        {
            //Make a StringBuilder, to display final message
            StringBuilder FinalMessage = new StringBuilder();

            //If username is changed...
            if (Name != IoC.Get<ApplicationViewModel>().ApplicationUser.Username)
            {
                //Create new HttpResponseMessage and set StatusCode to NotImplemented
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotImplemented
                };

                //If name length is less that four...
                if (Name.Length < 4)
                {
                    //Add error to Final message
                    FinalMessage.Append($"Name not changed, reason: Name must contains at least 5 letters. ");
                }
                //Otherwise...
                else
                {
                    //Send request to the API
                    response = await WebRequests.PostAsync(UserRoutes.ChangeUsername, new ChangeUsernameRequest
                    {
                        NewUsername = Name
                    }, IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);
                }

                //if response status code is OK...
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Read context as ApiResponse<CredentialChanged>
                    var context = response.DeseralizeHttpResponse<ApiResponse<CredentialChangedResponse>>();

                    //If ApiResponse is successful
                    if (context.IsSuccessful)
                    {
                        //Set header name to new username
                        HeaderName = Name;

                        //Set ApplicationUser Username to new username
                        IoC.Get<ApplicationViewModel>().ApplicationUser.Username = Name;

                        //Get new JWTToken
                        IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken = context.TResponse.JWTToken;

                        //Add message to FinalMessage
                        FinalMessage.Append($"Name changed successfully. ");
                    }
                    //Otherwise...
                    else
                    {
                        //Add error to Final message
                        FinalMessage.Append($"Name not changed, reason: {context.ErrorMessage} ");
                    }
                }
            }

            //If Email is changed...
            if (Email != IoC.Get<ApplicationViewModel>().ApplicationUser.Email)
            {

                //Create new HttpResponseMessage and set StatusCode to NotImplemented
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotImplemented
                };

                //If Email length is less that four...
                if (Email.Length < 4)
                {
                    //Add error to Final message
                    FinalMessage.Append($"Email not changed, reason: Email must contains at least 5 letters. ");
                }
                //If email does not have @ letter
                else if (!(Email.Contains('@')))
                {
                    //Add error to Final message
                    FinalMessage.Append($"Email not changed, reason: Email is not valid ");
                }
                //Otherwise...
                else
                {
                    //Send request to the API
                    response = await WebRequests.PostAsync(UserRoutes.ChangeEmail, new ChangeEmailRequest
                    {
                        NewEmail = Email
                    }, IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);
                }

                //if response status code is OK...
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Read context as ApiResponse<CredentialChanged>
                    var context = response.DeseralizeHttpResponse<ApiResponse<CredentialChangedResponse>>();

                    //If ApiResponse is successful
                    if (context.IsSuccessful)
                    {
                        //Set ApplicationUser Email to new Email
                        IoC.Get<ApplicationViewModel>().ApplicationUser.Email = Email;

                        //Get new JWTToken
                        IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken = context.TResponse.JWTToken;

                        //Add error to Final message
                        FinalMessage.Append($"Email changed successfully. ");
                    }
                    //Otherwise...
                    else
                    {
                        //Add error to Final message
                        FinalMessage.Append($"Email not changed, reason: {context.ErrorMessage} ");
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
                    var response = await WebRequests.PostAsync(UserRoutes.ChangePassword, new ChangePasswordRequest
                    {
                        CurrentPassword = (credentials as SettingsPage).CurrentPassword.SecurePassword.Unsecure(),
                        NewPassword = (credentials as SettingsPage).Password.SecurePassword.Unsecure()
                    }, IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

                    //if response status code is OK...
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //Read context as ApiResponse<CredentialChanged>
                        var context = response.DeseralizeHttpResponse<ApiResponse<CredentialChangedResponse>>();

                        //If ApiResponse is successful
                        if (context.IsSuccessful)
                        {
                            //Get new JWTToken
                            IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken = context.TResponse.JWTToken;

                            //Add error to Final message
                            FinalMessage.Append("Password changed successfully. ");

                            //Show FinalMessage
                            PopupExtensions.NewPopupWithMessage(FinalMessage.ToString());
                        }
                        //Otherwise...
                        else
                        {
                            //Add error to Final message
                            FinalMessage.Append($"Password not changed, reason: {context.ErrorMessage}. ");

                            //Show FinalMessage
                            PopupExtensions.NewPopupWithMessage(FinalMessage.ToString());
                        }
                    }
                }
                //Otherwise...
                else
                {
                    //Add error to Final message
                    FinalMessage.Append("Password not changed, reason: New password, and confirm new password as different");

                    //Show FinalMessage
                    PopupExtensions.NewPopupWithMessage(FinalMessage.ToString());
                }

            }
            //Otherwise...
            else
            {
                //Show FinalMessage
                PopupExtensions.NewPopupWithMessage(FinalMessage.ToString());
            }
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
        private async void UploadNewPhoto()
        {
            //Create new OpenFileDialog
            var Dialog = new Microsoft.Win32.OpenFileDialog();

            //Open new dialog, await to choose a file, or close window
            Dialog.ShowDialog();

            //Set pathToImage to the given path...
            var pathToImage = Dialog.FileName;

            //If path is not empty or null...
            if (!(string.IsNullOrEmpty(pathToImage)))
            {
                //Take a image from PC
                Image imageFromPC = Image.FromFile(pathToImage);

                //Create new Model, which will be sent to API
                var ImageToChange = new ChangeImageRequest
                {

                    //Set Image in model, as byte[] image
                    Image = imageFromPC.ImageToBytes()
                };

                //Send request to the API
                var response = await WebRequests.PostAsync(UserRoutes.ChangeImage,
                    ImageToChange,
                    IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

                //if response status code is OK...
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Read context as ApiResponse<CredentialChanged>
                    var context = response.DeseralizeHttpResponse<ApiResponse<CredentialChangedResponse>>();

                    //If ApiResponse is successful
                    if (context.IsSuccessful)
                    {
                        //Set current image of user, to new image
                        ImageBytes = imageFromPC.ImageToBytes();

                        //Set ApplicationUser Image, to new image
                        IoC.Get<ApplicationViewModel>().ApplicationUser.Image = imageFromPC.ImageToBytes();
                    }
                    //Otherwise...
                    else
                    {
                        //Show error message
                        PopupExtensions.NewPopupWithMessage(context.ErrorMessage);
                    }
                }
            }
        }

        #endregion
    }
}
