namespace ManagementBook.Application.Features.Books.Handlers;

using LanguageExt.Common;
using ManagementBook.Application.Features.Books.Queries;
using ManagementBook.Domain.Books;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class BookByIdHandler : IRequestHandler<BookByIdQuery, Result<Book>>
{
    private IBookRepository _bookRepository;

    public BookByIdHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public Task<Result<Book>> Handle(BookByIdQuery request, CancellationToken cancellationToken)
        => _bookRepository.GetById(request.Guid);
}
