namespace Toute.Core
{
    public class ApiResponse
    {
        public bool IsSucessfull => ErrorMessage == null;
        public string ErrorMessage { get; set; }

        public object Response { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T TResponse { get; set; }
    }
}
