using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Toute.Core
{
    public static class WebExtensions
    {
        public static async Task<ApiResponse> DeseralizeApiResponseAsync(this HttpResponseMessage message)
        {
            var messageContent = await message.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ApiResponse>(messageContent);

            return content;
        }

        public static async Task<ApiResponse<T>> DeseralizeTApiResponseAsync<T>(this HttpResponseMessage message)
        {
            var messageContent = await message.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ApiResponse<T>>(messageContent);

            return content;
        }
    }
}
