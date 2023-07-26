namespace TinyCRM.API.Exceptions
{
    public class NotFoundHttpException : HttpException
    {
        public NotFoundHttpException(string message) : base(StatusCodes.Status404NotFound, "Not Found", message)
        {
        }
    }
}