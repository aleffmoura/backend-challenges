namespace ManagementBook.Application.Features.Books.Commands;

using LanguageExt.Common;
using MediatR;
using Unit = LanguageExt.Unit;

public class BookRemoveCommand : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }

    public BookRemoveCommand(Guid id)
    {
        Id = id;
    }
}
