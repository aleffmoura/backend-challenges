namespace ManagementBook.Infra.Cross.Errors;
public class InvalidObjectError : BaseError
{
    public InvalidObjectError(string message) : base(errorCode: ECodeError.InvalidObject, message)
    {

    }
}
