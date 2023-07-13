namespace TinyCRM.API.Exceptions
{
    public abstract class HttpException : Exception
    {
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; }
        public HttpException(int statusCode, string errorCode, string message) : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}
