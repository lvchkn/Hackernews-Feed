namespace Shared.Exceptions;

public class StoryNotFoundException : Exception
{
    public StoryNotFoundException()
    {
    }

    public StoryNotFoundException(string message) : base(message)
    {
    }

    public StoryNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}