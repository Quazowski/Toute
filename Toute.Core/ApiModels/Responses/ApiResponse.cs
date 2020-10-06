using System.Collections.Generic;
using System.Linq;

namespace Toute.Core
{
    public class ApiResponse
    {
        public bool IsSucessfull => string.IsNullOrEmpty(ErrorMessage);
        public List<string> ErrorMessages { get; set; }
        public string ErrorMessage => ErrorMessages.FirstOrDefault();
        public object Response { get; set; }

        public ApiResponse()
        {
            ErrorMessages = new List<string>();
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T TResponse { get; set; }
    }
}
