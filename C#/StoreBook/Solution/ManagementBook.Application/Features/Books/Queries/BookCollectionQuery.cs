namespace ManagementBook.Application.Features.Books.Queries;

using LanguageExt.Common;
using ManagementBook.Domain.Books;
using MediatR;
using System.Linq;

public class BookCollectionQuery : IRequest<Result<IQueryable<Book>>>
{
}
