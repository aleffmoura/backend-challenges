namespace ManagementBook.Application.Features.Books.Commands;

using FluentValidation;
using FluentValidation.Results;
using LanguageExt.Common;
using MediatR;
using System;
using Unit = LanguageExt.Unit;

public class BookSaveCommand : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
    public string? Author { get; set; }
    public string? Title { get; set; }
    public DateTime Released { get; set; }

    public ValidationResult Validate()
        => new BookSaveCommandValidator().Validate(this);

    private class BookSaveCommandValidator : AbstractValidator<BookSaveCommand>
    {
        public BookSaveCommandValidator()
        {
            RuleFor(a => a.Author)
                    .NotEmpty()
                    .WithMessage("Author cant be null or empty")
                    .Must(x => x is not null && x.Length > 2)
                    .WithMessage("Author is less than 2.");

            RuleFor(a => a.Title)
                    .NotEmpty()
                    .WithMessage("Title cant be null or empty")
                    .Must(x => x is not null && x.Length > 2)
                    .WithMessage("Title is less than 2.");

            RuleFor(a => a.Released)
                    .Must( x => x > DateTime.MinValue)
                    .WithMessage("Release date is invalid");
        }
    }
}
