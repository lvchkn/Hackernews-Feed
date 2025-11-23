namespace Shared.Exceptions;

public class StoryAlreadyExistsException : Exception
{
    public StoryAlreadyExistsException()
    {
    }

    public StoryAlreadyExistsException(string message) : base(message)
    {
    }

    public StoryAlreadyExistsException(string message, Exception inner) : base(message, inner)
    {
    }
}