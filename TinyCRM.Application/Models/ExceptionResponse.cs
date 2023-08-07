namespace TinyCRM.Application.Models;

public class ExceptionResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; } = "";
}