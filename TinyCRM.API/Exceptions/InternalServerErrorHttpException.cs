namespace TinyCRM.API.Exceptions
{
    public class InternalServerErrorHttpException : HttpException
    {
        public InternalServerErrorHttpException(string message) : base(StatusCodes.Status500InternalServerError, "Internal Server Error", message)
        {
        }
    }
}