namespace ManagementBook.Api.Behaviors;

using FluentValidation;
using LanguageExt.Common;
using MediatR;

public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>> where TRequest : notnull
{
    private readonly IValidator<TRequest>[] _validators;
    public ValidatorBehavior(IValidator<TRequest>[] validators) => _validators = validators;

    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next, CancellationToken cancellationToken)
    {
        List<FluentValidation.Results.ValidationFailure> failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        return failures.Any()
               ? new Result<TResponse>(new ValidationException(failures))
               : await next();
    }
}

