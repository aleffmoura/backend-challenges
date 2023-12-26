namespace ManagementBook.Domain.Books;

using LanguageExt;
using LanguageExt.Common;

public interface IBookRepository
{
    Task<IQueryable<Book>> GetAll();
    Task<Result<Book>> GetById(Guid id);
    Task<Result<Unit>> Save(Option<Book> book);
    Task<Result<Unit>> Update(Option<Book> book);
    Task<Result<Unit>> Remove(Guid guid);
}
