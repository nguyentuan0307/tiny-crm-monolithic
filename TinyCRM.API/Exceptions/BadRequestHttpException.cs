namespace TinyCRM.API.Exceptions
{
    public class BadRequestHttpException : HttpException
    {
        public BadRequestHttpException(string message) : base(400, "Bad Request", message) { }
    }
}
