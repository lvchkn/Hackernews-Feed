namespace HackerNewsCommentsFeed.Utils;

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException()
    {
    }

    public AlreadyExistsException(string message) : base(message)
    {
    }

    public AlreadyExistsException(string message, Exception inner) : base(message, inner)
    {
    }
}