namespace ManagementBook.Domain.Books;

using FluentValidation.Results;
using System.Collections.Generic;

public interface IBookDomainService
{
    IEnumerable<ValidationFailure> Validate(Book book);
}
