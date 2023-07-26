namespace TinyCRM.API.Exceptions
{
    public class BadRequestHttpException : HttpException
    {
        public BadRequestHttpException(string message) : base(StatusCodes.Status400BadRequest, "Bad Request", message)
        {
        }
    }
}