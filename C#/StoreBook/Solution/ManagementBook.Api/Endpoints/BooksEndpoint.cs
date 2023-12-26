namespace ManagementBook.Api.Endpoints;

using AutoMapper;
using FluentValidation;
using ManagementBook.Api.DTOs;
using ManagementBook.Api.ViewModels;
using ManagementBook.Application.Features.Books.Commands;
using ManagementBook.Application.Features.Books.Queries;
using ManagementBook.Domain.Books;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using static BaseEndpointMethod;

public static class BooksEndpoint
{

    const string _baseEndpoint = "Books";
    public static WebApplication BookGetEndpoint(this WebApplication app)
    {
        app.MapGet(_baseEndpoint,
                   async ([FromServices] IMediator mediator,
                          [FromServices] IMapper mapper) =>
                   {
                       var returned = await mediator.Send(new BookCollectionQuery());

                       return HandleQueryable<Book, BookDetailViewModel>(returned, mapper);
                   }
        ).WithName($"Get{_baseEndpoint}")
        .WithOpenApi();

        return app;
    }

    public static WebApplication BookGetByIdEndpoint(this WebApplication app)
    {
        app.MapGet($"{_baseEndpoint}/{{id}}",
                   async ([FromServices] IMediator mediator,
                          [FromServices] IMapper mapper,
                          [FromRoute] Guid id) =>
                   {
                       var returned = await mediator.Send(new BookByIdQuery(id));

                       return HandleQuery<Book, BookDetailViewModel>(returned, mapper);
                   }
        ).WithName($"GetById{_baseEndpoint}")
        .WithOpenApi();

        return app;
    }

    public static WebApplication BookPostEndpoint(this WebApplication app)
    {
        app.MapPost($"{_baseEndpoint}",
                   async ([FromServices] IMediator mediator,
                          [FromServices] IMapper mapper,
                          [FromBody] BookCreateDto createDto) =>
                   {
                       return HandleCommand(await mediator.Send(mapper.Map<BookSaveCommand>(createDto)));
                   }
        ).WithName($"Post{_baseEndpoint}")
        .WithOpenApi();

        return app;
    }

    public static WebApplication BookPutEndpoint(this WebApplication app)
    {
        app.MapPut($"{_baseEndpoint}/{{id}}",
                   async ([FromServices] IMediator mediator,
                          [FromServices] IMapper mapper,
                          [FromRoute] Guid id,
                          [FromBody] BookUpdateDto updateDto) =>
                   {
                       return HandleCommand(await mediator.Send(mapper.Map<BookUpdateCommand>((id, updateDto))));
                   }
        ).WithName($"Put{_baseEndpoint}")
        .WithOpenApi();

        return app;
    }

    public static WebApplication BookPatchEndpoint(this WebApplication app)
    {
        app.MapPatch($"{_baseEndpoint}/{{id}}",
                   async ([FromServices] IMediator mediator,
                          [FromServices] IMapper mapper,
                          [FromRoute] Guid id,
                          [FromForm] IFormFile file) =>
                   {
                       using var memoryStream = new MemoryStream();
                       await file.CopyToAsync(memoryStream);

                       var dto = new BookCoverPatchDto() { Data = memoryStream.ToArray() };
                       return HandleAccepted(await mediator.Send(mapper.Map<BookPatchCommand>((id, dto))));
                   }
        ).WithName($"Patch{_baseEndpoint}")
        .WithOpenApi()
        .WithMetadata(new RequireAntiforgeryTokenAttribute(false));

        return app;
    }

}
