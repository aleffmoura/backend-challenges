namespace ManagementBook.Application.Features.Books.Handlers;

using AutoMapper;
using LanguageExt.Common;
using ManagementBook.Application.Features.Books.Commands;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Cross.Errors;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Unit = LanguageExt.Unit;

public class BookUpdateHandler : IRequestHandler<BookUpdateCommand, Result<Unit>>
{
    private IMapper _mapper;
    private IBookRepository _bookRepository;

    public BookUpdateHandler(IMapper mapper, IBookRepository bookRepository)
    {
        _mapper = mapper;
        _bookRepository = bookRepository;
    }

    public async Task<Result<Unit>> Handle(BookUpdateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var book = _mapper.Map<Book>(request);
            return await _bookRepository.Update(book);
        }
        catch (Exception ex)
        {
            return new Result<Unit>(new InternalError("Erro while update book, contact admin", ex));
        }
    }

}
