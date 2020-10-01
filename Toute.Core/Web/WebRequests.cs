using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Toute.Core
{
    public static class WebRequests
    {
        private static readonly HttpClient client = new HttpClient();
        public static async Task<HttpResponseMessage> PostAsync(string url, object message)
        {
            var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

            return response;
        }
    }
}
