namespace TinyCRM.API.Exceptions
{
    public class NotFoundHttpException : HttpException
    {
        public NotFoundHttpException(string message) : base(404, "Not Found", message)
        {
        }
    }
}