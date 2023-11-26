namespace Shared.Exceptions;

public class QueryParameterException : Exception
{
    public QueryParameterException()
    {
    }

    public QueryParameterException(string message) : base(message)
    {
    }

    public QueryParameterException(string message, Exception inner) : base(message, inner)
    {
    }
}