namespace ManagementBook.Application.Features.Books.Handlers;

using AutoMapper;
using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Application.Features.Books.Commands;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Cross.Errors;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static LanguageExt.Prelude;
using Unit = LanguageExt.Unit;

public class BookSaveHandler : IRequestHandler<BookSaveCommand, Result<Unit>>
{
    private IMapper _mapper;
    private IBookRepository _bookRepository;

    public BookSaveHandler(IMapper mapper, IBookRepository bookRepository)
    {
        _mapper = mapper;
        _bookRepository = bookRepository;
    }

    public async Task<Result<Unit>> Handle(BookSaveCommand request, CancellationToken cancellationToken)
        => await TryAsync(async () =>
        {
            var book = _mapper.Map<Book>(request);

            return await _bookRepository.Save(book);
        }).IfFail(fail => new Result<Unit>(new InternalError("Erro on book save, contect admin.")));
}
