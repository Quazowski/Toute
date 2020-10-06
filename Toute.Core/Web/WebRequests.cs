using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Toute.Core
{
    public static class WebRequests
    {
        private static readonly HttpClient client = new HttpClient();
        public static async Task<HttpResponseMessage> PostAsync(string url, object message, string JWTToken = null)
        {
            var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

            if(!(string.IsNullOrEmpty(JWTToken)))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            }

            var response = await client.PostAsync(url, content);
            
            return response;
        }
    }
}
