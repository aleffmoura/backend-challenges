namespace ManagementBook.Application.Features.Books.Handlers;

using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Application.Features.Books.Commands;
using ManagementBook.Application.Notifications.Books;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Unit = LanguageExt.Unit;

public class BookCoverPatchHandler : IRequestHandler<BookPatchCommand, Result<Unit>>
{
    private IMediator _mediator;

    public BookCoverPatchHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<Result<Unit>> Handle(BookPatchCommand request, CancellationToken cancellationToken)
    {
        _mediator.Publish(new BookCoverNotification
        {
            BookId = request.BookId,
            Data = request.Data
        });

        return new Result<Unit>(Unit.Default).AsTask();
    }
}
