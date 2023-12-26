namespace ManagementBook.Application.Features.Books.Commands;

using FluentValidation;
using FluentValidation.Results;
using LanguageExt;
using LanguageExt.Common;

using MediatR;
using Unit = LanguageExt.Unit;

public class BookPatchCommand : IRequest<Result<Unit>>
{
    public Guid BookId { get; set; }
    public byte[] Data { get; set; }

    public ValidationResult Validate()
        => new BookPatchCommandValidator().Validate(this);

    private class BookPatchCommandValidator : AbstractValidator<BookPatchCommand>
    {
        public BookPatchCommandValidator()
        {
            RuleFor(a => a.BookId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("BookId is incorrect.");

            RuleFor(a => a.Data)
                    .Must(a => a.Count() > 0)
                    .WithMessage("Data byte should be greater than 0.");
        }
    }
}
