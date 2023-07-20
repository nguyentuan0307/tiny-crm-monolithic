namespace TinyCRM.API.Exceptions
{
    public class InternalServerErrorHttpException : HttpException
    {
        public InternalServerErrorHttpException(string message) : base(500, "Internal Server Error", message)
        {
        }
    }
}