namespace Shared.Exceptions;

public class InterestAlreadyExistsException : Exception
{
    public InterestAlreadyExistsException()
    {
    }

    public InterestAlreadyExistsException(string message) : base(message)
    {
    }

    public InterestAlreadyExistsException(string message, Exception inner) : base(message, inner)
    {
    }
}