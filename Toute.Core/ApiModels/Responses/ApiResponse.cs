using System.Collections.Generic;
using System.Linq;

namespace Toute.Core
{
    /// <summary>
    /// Default response for all API calls
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// It signals, if response is successful 
        /// If there is no errors, return true, otherwise false
        /// </summary>
        public bool IsSuccessful => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// All error messages, from response
        /// </summary>
        public List<string> ErrorMessages { get; set; }

        /// <summary>
        /// First error message, from <see cref="ErrorMessages"/>
        /// </summary>
        public string ErrorMessage => ErrorMessages.FirstOrDefault();

        /// <summary>
        /// Message that contain information about response
        /// </summary>
        public object Response { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiResponse()
        {
            //Create new ErrorMessages list
            ErrorMessages = new List<string>();
        }
    }

    /// <summary>
    /// Generic response from API
    /// </summary>
    /// <typeparam name="T">Type of the response</typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// Generic response
        /// </summary>
        public T TResponse { get; set; }
    }
}
