using System;

namespace Toute.Core
{
    /// <summary>
    /// Response when exception occur
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Type of the exception
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Message of the exception
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Stack trace of the exception
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="ex">Exception that occurred</param>
        public ErrorResponse(Exception ex)
        {
            //Assign exception to fields
            Type = ex.GetType().Name;
            Message = ex.Message;
            StackTrace = ex.ToString();
        }
    }
}
