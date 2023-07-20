namespace TinyCRM.API.Model
{
    public class ExceptionRespone
    {
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; } = "";
        public string Message { get; set; } = "";
    }
}