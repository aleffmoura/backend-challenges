namespace ManagementBook.Api;

using AutoMapper;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Infra.Cross.Errors;
using System.Net;
using System.Text.Json;

public static class BaseEndpointMethod
{
    public static IResult HandleAccepted<TSource>(Result<TSource> result)
        => result.Match(succ => Results.Accepted(), error => HandleFailure(error));
    public static IResult HandleCommand<TSource>(Result<TSource> result)
        => result.Match(succ => Results.Ok(succ), error => HandleFailure(error));

    public static IResult HandleQuery<TSource, TDestiny>(Result<TSource> result, IMapper m)
        => result.Match(succ => Results.Ok(m.Map<TDestiny>(succ)), error => HandleFailure(error));
    public static IResult HandleQueryable<TSource, TDestiny>(Result<IQueryable<TSource>> result, IMapper m)
        => result.Match(succ => Results.Ok(m.ProjectTo<TDestiny>(succ, m.ConfigurationProvider)), error => HandleFailure(error));

    private static IResult HandleFailure<T>(T exception) where T : Exception
        => exception is ValidationException validationError
            ? Results.Problem(detail: JsonSerializer.Serialize(validationError.Errors), statusCode: HttpStatusCode.BadRequest.GetHashCode())
            : ErrorPayload.New(exception).Apply(error => Results.Problem(detail: JsonSerializer.Serialize(error), statusCode: error.ErrorCode.GetHashCode()));
}
