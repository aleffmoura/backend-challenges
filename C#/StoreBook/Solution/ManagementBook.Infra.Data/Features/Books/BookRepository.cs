namespace ManagementBook.Infra.Data.Features.Books;

using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pretty;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Cross.Errors;
using ManagementBook.Infra.Data.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

public class BookRepository : IBookRepository
{
    private BookStoreContext _baseContext;

    public BookRepository(BookStoreContext baseContext)
    {
        _baseContext = baseContext;
    }

    public async Task<Result<Book>> GetById(Guid id)
        => await TryAsync(async () =>
                ( await _baseContext.Books.FindAsync(id) )
                .Apply(book =>
                {
                    var attached = _baseContext.Attach(book);
                    if (attached is not null)
                        attached.State = EntityState.Detached;
                    return book;
                })
                .Apply(book => book is null
                       ? new Result<Book>(new NotFoundError($"Book with {{id}}: {id} not found."))
                       : book))
           .IfFail(fail => new Result<Book>(new InternalError("Error on GetById, contact the admin.", fail)));

    public Task<IQueryable<Book>> GetAll()
        => _baseContext.AsNoTracking(_baseContext.Books.AsQueryable()).AsTask();

    public async Task<Result<Unit>> Remove(Guid guid)
    {
        try
        {
            Book? book = await _baseContext.Books.FindAsync(guid);

            var removeAction = async (Book b) =>
            {
                _ = _baseContext.Remove(b);
                _ = await _baseContext.SaveChangesAsync();
                return unit;
            };

            return book is null
                   ? new Result<Unit>(new NotFoundError($"Book with {{id}}: {guid} not found."))
                   : await removeAction(book);
        }
        catch (Exception ex)
        {
            return new Result<Unit>(new InternalError($"Error on Remove, contact the admin.", ex));
        }
    }

    public async Task<Result<Unit>> Save(Option<Book> maybeBook)
    {
        try
        {
            return await maybeBook.MatchAsync(async book =>
            {
                _ = await _baseContext.Books.AddAsync(book);
                _ = await _baseContext.SaveChangesAsync();
                return new Result<Unit>(unit);
            }, () => new Result<Unit>(new InvalidObjectError("Book cant be null.")).AsTask());
        }
        catch (Exception ex)
        {
            return new Result<Unit>(new InternalError("Error while try save on DB, contact admin.", ex));
        }
    }

    public async Task<Result<Unit>> Update(Option<Book> maybeBook)
        => await TryAsync(async () => await maybeBook.MatchAsync(async book =>
            {
                Book? bookOnDB = _baseContext.AsNoTracking(_baseContext.Books).FirstOrDefault(f => f.Id == book.Id);

                var saveAction = async (Book toUpdateBook) =>
                {
                    _ = _baseContext.Books.Update(toUpdateBook);
                    _ = await _baseContext.SaveChangesAsync();
                    return unit;
                };

                return bookOnDB is not null
                       ? await saveAction(bookOnDB with
                       {
                           Author = book.Author,
                           Title = book.Title,
                           ReleaseDate = book.ReleaseDate,
                           BookCoverUrl = book.BookCoverUrl
                       })
                       : new Result<Unit>(new NotFoundError($"Book with {{id}}:{book.Id} notfound"));

            }, () => new Result<Unit>(new InvalidObjectError("Book cant be null.")).AsTask()))
        .IfFail(fail => new Result<Unit>(new InternalError("Error while try save on DB, contact admin.", fail)));
}
