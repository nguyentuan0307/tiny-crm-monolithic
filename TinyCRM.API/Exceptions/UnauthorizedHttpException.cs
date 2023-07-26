namespace TinyCRM.API.Exceptions
{
    public class UnauthorizedHttpException : HttpException
    {
        public UnauthorizedHttpException(string message) : base(StatusCodes.Status401Unauthorized, "Unauthorized", message)
        {
        }
    }
}