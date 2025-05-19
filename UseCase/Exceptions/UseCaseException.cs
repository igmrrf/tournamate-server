
using System.Runtime.Serialization;

namespace UseCase.Exceptions;

public class UseCaseException : Exception
{
    public UseCaseException(string message, string errorCode, int statusCode, object? error = null) : base(message)
    {
        HttpStatusCode = statusCode;
        Error = error;
        ErrorCode = errorCode;
    }

    public UseCaseException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected UseCaseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public int HttpStatusCode { get; init; }
    public object? Error { get; init; }
    public string ErrorCode { get; init; } = default!;
}