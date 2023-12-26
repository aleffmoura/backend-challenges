namespace ManagementBook.Infra.Cross.Errors;
using System;

public class BaseError : Exception
{
    public ECodeError ErrorCode { get; }

    public BaseError(ECodeError errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public BaseError(ECodeError errorCode, string message, Exception inner) : base(message, inner)
    {
        ErrorCode = errorCode;
    }
}
