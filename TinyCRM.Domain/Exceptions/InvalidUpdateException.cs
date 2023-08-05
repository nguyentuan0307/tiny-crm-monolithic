namespace TinyCRM.Domain.Exceptions;

public class InvalidUpdateException : Exception
{
    public InvalidUpdateException(string message) : base(message)
    {
    }

    public InvalidUpdateException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public InvalidUpdateException()
    {
    }
}