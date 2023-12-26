namespace ManagementBook.Application.Features.Books.Handlers;

using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Application.Features.Books.Queries;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Cross.Errors;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

public class BookCollectionHandler : IRequestHandler<BookCollectionQuery, Result<IQueryable<Book>>>
{
    private IBookRepository _bookRepository;

    public BookCollectionHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Result<IQueryable<Book>>> Handle(BookCollectionQuery request, CancellationToken cancellationToken)
    => await TryAsync(
        async () => new Result<IQueryable<Book>>(await _bookRepository.GetAll())
    ).IfFail(fail => new Result<IQueryable<Book>>(new InternalError("Error on DB, please contact the admin.")));
}
