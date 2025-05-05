using System.Net;

namespace Orders.CrossCutting.Exceptions
{
    public class GlobalException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public object? Details { get; }

        public GlobalException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, object? details = null)
            : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }

        public GlobalException(string message, Exception innerException, HttpStatusCode statusCode = HttpStatusCode.BadRequest, object? details = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }
}
