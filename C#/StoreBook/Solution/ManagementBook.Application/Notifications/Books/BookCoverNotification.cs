namespace ManagementBook.Application.Notifications.Books;

using MediatR;

public record BookCoverNotification : INotification
{
    public required Guid BookId { get; init; }
    public required byte[] Data { get; init; }
}
