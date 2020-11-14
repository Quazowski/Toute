using NLog;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Toute.Core;
using Toute.Core.Routes;
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
        private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Method that will send request to the API, and 
        /// will except to return Response.
        /// </summary>
        /// <param name="url">URL where to make a call to API</param>
        /// <param name="RequestModel">Model that will be sent with the request</param>
        /// <returns>Returns boolean. If request is successful
        /// it will return true, otherwise false</returns>
        public static async Task<bool> HandleHttpRequestAsync(string url, object RequestModel)
        {
            _logger.Debug($"Requesting API to URL: {url}");
            //Get user token
            var token = ViewModelApplication.ApplicationUser?.Token;

            try
            {
                //Make a request to API
                var response = await WebRequests.PostAsync(url, RequestModel, token);
                _logger.Debug($"Got response from the server from URL: {url}. Response is of StatusCode: {response.StatusCode}");
                ViewModelApplication.InternetHealth = false;
                ViewModelApplication.ServerHealth = false;

                //If response status code is OK...
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Read context as ApiResponse
                    var context = await response.DeseralizeHttpResponseOfT<ApiResponse>();

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
                    //Check if the token is expired, or user want to access to the page, where he is not allowed
                    if (response.Headers.FirstOrDefault(x => x.Key == "Token-Expired").Value != null)
                    {
                        //If token was expired, and user got new token request page again
                        if (await RefreshTokenAsync())
                            return await HandleHttpRequestAsync(url, RequestModel);
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    _logger.Warn($"Internal server error from URL: {url}");

                    //Show error message...
                    PopupExtensions.NewErrorPopup("Server not responding, try again later.");
                }
                else
                {
                    _logger.Warn($"Error when requesting {url}");
                }
            }
            //If server is not responding...
            catch (HttpRequestException e)
            {
                try
                {
                    using (var client = new WebClient())
                    using (client.OpenRead("http://google.com/generate_204"))

                    ViewModelApplication.InternetHealth = true;
                    _logger.Error(e);

                }
                catch
                {
                    ViewModelApplication.ServerHealth = true;
                    _logger.Debug(e);

                }

            }
            catch (Exception e)
            {
                _logger.Error(e);
                PopupExtensions.NewErrorPopup("Can not request server. Try again later.");
            }

            //If try statement failed, return false.
            return false;

        }

        /// <summary>
        /// Method that will send request to the API, and 
        /// will except to return Response of T type.
        /// </summary>
        /// <typeparam name="T">The type of expected return type</typeparam>
        /// <param name="url">URL where to make a call to API</param>
        /// <param name="RequestModel">Model that will be sent with the request</param>
        /// <returns></returns>
        public static async Task<T> HandleHttpRequestOfTResponseAsync<T>(string url, object RequestModel)
        {
            _logger.Debug($"Requesting API to URL: {url}");

            //Get user token
            var token = ViewModelApplication.ApplicationUser?.Token;

            try
            {
                //Make a request to API
                var response = await WebRequests.PostAsync(url, RequestModel, token);
                _logger.Debug($"Got response from the server from URL: {url}. Response is of StatusCode: {response.StatusCode}");
                ViewModelApplication.InternetHealth = false;
                ViewModelApplication.ServerHealth = false;
                //If response status code is OK and/or there is a context back...
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    
                    //Read context as ApiResponse<T>
                    var context = await response.DeseralizeHttpResponseOfT<ApiResponse<T>>();

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
                    //Check if the token is expired, or user want to access to the page, where he is not allowed
                    if (response.Headers.FirstOrDefault(x => x.Key == "Token-Expired").Value != null)
                    {
                        //If token was expired, and user got new token request page again
                        if (await RefreshTokenAsync())
                            return await HandleHttpRequestOfTResponseAsync<T>(url, RequestModel);
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    _logger.Warn($"Internal server error from URL: {url}");

                    //Show error message...
                    PopupExtensions.NewErrorPopup("Server not responding, try again later.");
                }
                else
                {
                    _logger.Warn($"Error when requesting {url}");
                }
            }
            catch (HttpRequestException e)
            {
                try
                {
                    using (var client = new WebClient())
                    using (client.OpenRead("http://google.com/generate_204"))
                    ViewModelApplication.InternetHealth = true;
                    _logger.Error(e);

                }
                catch
                {
                    ViewModelApplication.ServerHealth = true;
                    _logger.Debug(e);

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

        private static async Task<bool> RefreshTokenAsync()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                _logger.Debug($"Got Expired-Token response. Try to refresh token. Requesting API to URL: {UserRoutes.RefreshToken}");

                //Get user from DB
                var userFromLocalDB = await SqliteDb.GetLoginCredentialsAsync();

                //If no user was found in DB, our user it not logged, return false
                if (userFromLocalDB == null || ViewModelApplication.ApplicationUser == null)
                    return false;

                try
                {
                    //Make a request, to get new token
                    var response = await WebRequests.PostAsync(UserRoutes.RefreshToken, new RefreshTokenRequest
                    {
                        Token = ViewModelApplication.ApplicationUser.Token,
                        RefreshToken = userFromLocalDB.RefreshToken
                    });

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //If user responded OK, deseralize response
                        var AuthContext = await response.DeseralizeHttpResponseOfT<ApiResponse<TokenResponse>>();

                        //If response contain new tokens...
                        if (AuthContext.IsSuccessful)
                        {
                            _logger.Debug($"Response from  URL: {UserRoutes.RefreshToken} is successful. Saving new token to localDB");
                            //Set new token for a user
                            ViewModelApplication.ApplicationUser.Token = AuthContext.TResponse.Token;

                            //Set new refresh token for a user
                            await SqliteDb.ChangeUserTokensAsync(AuthContext.TResponse.Token, AuthContext.TResponse.RefreshToken);

                            return true;
                        }
                        else
                        {
                            await ViewModelApplication.LogoutAsync();
                            PopupExtensions.NewErrorPopup("Authorization failed. Please login again...");
                            _logger.Info($"Response: {UserRoutes.RefreshToken} returned failed status. Status code is: {response.StatusCode}. Did not refreshed token for a user.");
                            return false;
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        _logger.Error($"Internal server error from when requesting URL: {UserRoutes.RefreshToken}");

                        //Show error message...
                        PopupExtensions.NewErrorPopup("Server not responding, try again later.");

                        return false;
                    }
                    else
                    {
                        await ViewModelApplication.LogoutAsync();
                        PopupExtensions.NewErrorPopup("Authorization failed. Please login again...");
                        _logger.Info($"Response: {UserRoutes.RefreshToken} returned failed status. Status code is: {response.StatusCode}. Did not refreshed token for a user.");
                        return false;
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
                    PopupExtensions.NewErrorPopup("Can not request server. Try again later.");
                }

                return false;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }

}
