using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Toute
{
    /// <summary>
    /// Extensions for deseralize 
    /// </summary>
    public static class DeseralizeExtensions
    {
        /// <summary>
        /// Deseralize HttpResponse from json context to response of T.
        /// </summary>
        /// <typeparam name="T">Type of returning response</typeparam>
        /// <param name="httpResponse">HttpResponseMessage</param>
        /// <returns></returns>
        public static async Task<T> DeseralizeHttpResponse<T>(this HttpResponseMessage httpResponse)
        {
            //Takes a context from a response, and read it as json string.
            var httpContext = await httpResponse.Content.ReadAsStringAsync();

            //Converts json string, to the model of T
            T deserializedResult = JsonConvert.DeserializeObject<T>(httpContext);

            //returns a model as T type
            return deserializedResult;
        }
    }
}
