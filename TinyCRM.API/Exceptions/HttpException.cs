namespace TinyCRM.API.Exceptions
{
    public abstract class HttpException : Exception
    {
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; } = string.Empty;

        protected HttpException(string message) : base(message)
        {
        }

        protected HttpException(int statusCode, string errorCode, string message) : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}