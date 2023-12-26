namespace ManagementBook.Api.Behaviors;

using LanguageExt.Common;
using ManagementBook.Infra.Cross.Errors;
using MediatR;

public sealed class LoggingBehavior<TRequest, TResponse>
         : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, Result<TResponse>>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, Result<TResponse>>> logger)
    {
        _logger = logger;
    }

    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Handler {handlerName}", typeof(TRequest).Name);

        var response = await next();

        _logger.LogInformation("Request: {requestData} has response: {responseData}", request, response);

        response.IfFail(fail =>
        {
            if (fail is InternalError internalError)
            {
                _logger.LogCritical("InternalException handled");
                _logger.LogCritical("message: {messageException}, exception: {exception}", internalError.Message, internalError);
                _logger.LogCritical("InnerException: {innerException}", internalError.InnerException);
            }
            else if (fail is BaseError baseError)
            {
                _logger.LogError("BaseError handled");
                _logger.LogError("message: {messageError}, error: {error}", baseError.Message, baseError);
            }
            else
            {
                _logger.LogCritical("Exception handled");
                _logger.LogCritical("message: {messageException}, exception: {exception}", fail.Message, fail);
                _logger.LogCritical("InnerException: {innerException}", fail.InnerException);
            }
        });

        return response;
    }
}
