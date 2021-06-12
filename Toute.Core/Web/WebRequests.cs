using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Toute.Core
{
    /// <summary>
    /// A basic HTTP request, that application will use
    /// </summary>
    public static class WebRequests
    {
        /// <summary>
        /// Create new HttpClient
        /// </summary>
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Method that make asynchronous requests to API
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="body">Message that contains model of the request</param>
        /// <param name="JWTToken">Authorization token, if user is logged</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsync(string url, object body, string JWTToken = null)
        {
            //Create content, by serializing body to JsonString
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            //If token is not null...
            if(!(string.IsNullOrEmpty(JWTToken)))
            {
                //Add token to header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            }
            try
            {
                //Make request to API
                var response = await client.PostAsync(url, content);

                return response;
            }
            catch(Exception ex)
            {

            }
            //Make request to API
            var response1 = await client.PostAsync(url, content);
            
            return response1;
        }

        /// <summary>
        /// Method that make asynchronous requests to API
        /// from given client
        /// </summary>
        /// <param name="client">Http client</param>
        /// <param name="url">URL</param>
        /// <param name="body">Message that contains model of the request</param>
        /// <param name="JWTToken">Authorization token, if user is logged</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsync(this HttpClient client, string url, object body, string JWTToken = null)
        {
            //Create content, by serializing body to JsonString
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            //If token is not null...
            if (!(string.IsNullOrEmpty(JWTToken)))
            {
                //Add token to header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            }

            //Make request to API
            var response = await client.PostAsync(url, content);

            return response;
        }
    }
}
