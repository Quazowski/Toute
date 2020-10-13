using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Toute.Core;
using Toute.Extensions;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// Extension for HttpRequests
    /// </summary>
    public static class HttpExtensions
    {
        /// <summary>
        /// Method that will send request to the API, and 
        /// will except to return Response.
        /// </summary>
        /// <param name="url">Url where to make a call to API</param>
        /// <param name="RequestModel">Model that will be sent with the request</param>
        /// <returns>Returns boolean. If request is successful
        /// it will return true, otherwise false</returns>
        public static async Task<bool> HandleHttpRequestAsync(string url, object RequestModel)
        {
            //Get user token
            var token = ViewModelApplication.ApplicationUser?.JWTToken;

            try
            {
                //Make a request to API
                var response = await WebRequests.PostAsync(url, RequestModel, token);

                //If response status code is OK...
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Read context as ApiResponse
                    var context = response.DeseralizeHttpResponse<ApiResponse>();

                    //If there is successful response
                    if (context.IsSuccessful)
                    {
                        return true;
                    }
                    //if it is not...
                    else
                    {
                        PopupExtensions.NewInfoPopup(context.ErrorMessage);
                        return false;
                    }
                }
                //If there is no content back...
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return true;
                }
                //If user is not Unauthorized
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //Show error message...
                    PopupExtensions.NewErrorPopup("You are unauthorized. Please login to continue...");
                    //Logout if user is logged, do nothing if is not logged.
                    await ViewModelApplication.Logout();
                }
                //if any other error occurred...
                else
                {
                    return false;
                }
            }
            //If server is not responding...
            catch (HttpRequestException)
            {
                PopupExtensions.NewErrorPopup("There is a problem with server, or you network connection. Please try again later.");
            }
            //Catch any error, and display it to the user.
            catch (Exception ex)
            {
                PopupExtensions.NewErrorPopup(ex.Message);
            }

            //If try statement failed, return false.
            return false;

        }

        /// <summary>
        /// Method that will send request to the API, and 
        /// will except to return Response of T type.
        /// </summary>
        /// <typeparam name="T">The type of expected return type</typeparam>
        /// <param name="url">Url where to make a call to API</param>
        /// <param name="RequestModel">Model that will be sent with the request</param>
        /// <returns></returns>
        public static async Task<T> HandleHttpRequestOfTResponseAsync<T>(string url, object RequestModel)
        {
            //Get user token
            var token = ViewModelApplication.ApplicationUser?.JWTToken;

            try
            {
                //Make a request to API
                var response = await WebRequests.PostAsync(url, RequestModel, token);

                //If response status code is OK and/or there is a context back...
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Read context as ApiResponse<T>
                    var context = response.DeseralizeHttpResponse<ApiResponse<T>>();

                    //If there is successful response
                    if (context.IsSuccessful)
                    {
                        //return deseralize Response
                        return context.TResponse;
                    }
                    //Otherwise...
                    else
                    {
                        //Display error to the user
                        PopupExtensions.NewErrorPopup(context.ErrorMessage);
                    }
                }
                //If there is no content back, return null
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default;
                }
                //If user is not Unauthorized
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //Show error message...
                    PopupExtensions.NewErrorPopup("You are unauthorized. Please login to continue...");
                    //Logout if user is logged, do nothing if is not logged.
                    await ViewModelApplication.Logout();
                }
            }
            //If server is not responding...
            catch (HttpRequestException)
            {
                PopupExtensions.NewErrorPopup("There is a problem with server, or you network connection. Please try again later.");
            }
            //Catch any error, and display it to the user.
            catch (Exception ex)
            {
                PopupExtensions.NewErrorPopup(ex.Message);
            }

            //If try statement failed, return null
            return default;
        }
    }

}
