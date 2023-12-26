namespace ManagementBook.Api.Handlers;

using ManagementBook.Infra.Cross.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ErrorHandlerAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.Exception = context.Exception;
        context.HttpContext.Response.StatusCode = 500;
        context.Result = new JsonResult(ErrorPayload.New(context.Exception));
    }
}