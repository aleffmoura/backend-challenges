namespace ManagementBook.Infra.Cross.Errors;
public class NotFoundError : BaseError
{
    public NotFoundError(string message) : base(errorCode: ECodeError.NotFound, message)
    {

    }
}
