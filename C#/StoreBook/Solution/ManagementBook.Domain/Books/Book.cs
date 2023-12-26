namespace ManagementBook.Domain.Books;

using FluentValidation;
using System;

public record Book
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Author { get; init; }
    public required DateTime ReleaseDate { get; init; }
    public string? BookCoverUrl { get; init; }
}
