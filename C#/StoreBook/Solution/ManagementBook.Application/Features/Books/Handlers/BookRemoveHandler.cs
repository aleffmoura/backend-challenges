namespace ManagementBook.Application.Features.Books.Handlers;

using LanguageExt.Common;
using ManagementBook.Application.Features.Books.Commands;
using ManagementBook.Domain.Books;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Unit = LanguageExt.Unit;

public class BookRemoveHandler : IRequestHandler<BookRemoveCommand, Result<Unit>>
{
    private IBookRepository _bookRepository;
    public BookRemoveHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Result<Unit>> Handle(BookRemoveCommand request, CancellationToken cancellationToken)
        => await _bookRepository.Remove(request.Id);
}
