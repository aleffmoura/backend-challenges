namespace ManagementBook.Application.Features.Books.Queries;
using LanguageExt.Common;
using ManagementBook.Domain.Books;

using MediatR;

public class BookByIdQuery : IRequest<Result<Book>>
{
    public Guid Guid { get; init; }  

    public BookByIdQuery(Guid guid)
    {
        Guid = guid;
    }
}
