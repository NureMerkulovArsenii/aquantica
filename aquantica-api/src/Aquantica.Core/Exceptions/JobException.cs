namespace Aquantica.Core.Exceptions;

public class JobException : Exception
{
    public JobException(string message) : base(message)
    {
    }

    public JobException(string message, Exception innerException) : base(message, innerException)
    {
    }
}