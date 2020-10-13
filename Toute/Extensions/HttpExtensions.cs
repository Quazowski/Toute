using NLog;
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
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
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
            _logger.Debug($"Requesting API to url: {url}");
            //Get user token
            var token = ViewModelApplication.ApplicationUser?.JWTToken;

            try
            {
                //Make a request to API
                var response = await WebRequests.PostAsync(url, RequestModel, token);
                _logger.Debug($"Got response from the server from url: {url}. Response is of StatusCode: {response.StatusCode}");

                //If response status code is OK...
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Read context as ApiResponse
                    var context = response.DeseralizeHttpResponse<ApiResponse>();

                    //If there is successful response
                    if (context.IsSuccessful)
                    {
                        _logger.Debug($"Response: {url} returned successful status.");
                        return true;
                    }
                    //if it is not...
                    else
                    {
                        _logger.Debug($"Response: {url} returned failed status. Reason: {context.ErrorMessage}");
                        PopupExtensions.NewErrorPopup(context.ErrorMessage);
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
                    _logger.Debug("Unauthorized request to API");

                    //Show error message...
                    PopupExtensions.NewErrorPopup("You are not allowed to do this action..");
                    return false;
                }
                //if any other error occurred...
                else
                {
                    return false;
                }
            }
            //If server is not responding...
            catch (HttpRequestException e)
            {
                try
                {
                    using (var client = new WebClient())
                    using (client.OpenRead("http://google.com/generate_204"))

                        PopupExtensions.NewInfoPopup("Server currently is down, please try again later.");
                    _logger.Error(e);

                }
                catch
                {
                    PopupExtensions.NewErrorPopup("No network connection. Please check your net before continuing...");
                    _logger.Warn(e);

                }

            }
            catch (Exception e)
            {
                _logger.Error(e);
                PopupExtensions.NewErrorPopup("Error occurred, try again later.");
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
            _logger.Debug($"Requesting API to url: {url}");

            //Get user token
            var token = ViewModelApplication.ApplicationUser?.JWTToken;

            try
            {
                //Make a request to API
                var response = await WebRequests.PostAsync(url, RequestModel, token);
                _logger.Debug($"Got response from the server from url: {url}. Response is of StatusCode: {response.StatusCode}");

                //If response status code is OK and/or there is a context back...
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Read context as ApiResponse<T>
                    var context = response.DeseralizeHttpResponse<ApiResponse<T>>();

                    //If there is successful response
                    if (context.IsSuccessful)
                    {
                        _logger.Debug($"Response: {url} returned successful status.");
                        //return deseralize Response
                        return context.TResponse;
                    }
                    //Otherwise...
                    else
                    {
                        //Display error to the user
                        PopupExtensions.NewErrorPopup(context.ErrorMessage);
                        _logger.Debug($"Response: {url} returned failed status. Reason: {context.ErrorMessage}");
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
                    _logger.Debug("Unauthorized request to API");

                    //Show error message...
                    PopupExtensions.NewErrorPopup("You are not allowed to do this action..");

                }
            }
            catch (HttpRequestException e)
            {
                try
                {
                    using (var client = new WebClient())
                    using (client.OpenRead("http://google.com/generate_204")) 

                    PopupExtensions.NewInfoPopup("Server currently is down, please try again later.");
                    _logger.Error(e);

                }
                catch
                {
                    PopupExtensions.NewErrorPopup("No network connection. Please check your net before continuing...");
                    _logger.Warn(e);

                }

            }
            catch (Exception e)
            {
                _logger.Error(e);
                PopupExtensions.NewErrorPopup("Error occurred, try again later.");
            }

            //If try statement failed, return null
            return default;
        }
    }

}
