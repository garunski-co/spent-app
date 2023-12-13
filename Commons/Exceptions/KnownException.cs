namespace Spent.Commons.Exceptions;

public abstract class KnownException : Exception
{
    protected KnownException(string message)
        : base(message)
    {
        Key = message;
    }

    protected KnownException(string message, Exception? innerException)
        : base(message, innerException)
    {
        Key = message;
    }

    protected KnownException(LocalizedString message)
        : base(message.Value)
    {
        Key = message.Name;
    }

    protected KnownException(LocalizedString message, Exception? innerException)
        : base(message.Value, innerException)
    {
        Key = message.Name;
    }

    public string? Key { get; }
}
