namespace Shared.Exceptions;

public class InterestNotFoundException : Exception
{
    public InterestNotFoundException()
    {
    }

    public InterestNotFoundException(string message) : base(message)
    {
    }

    public InterestNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}