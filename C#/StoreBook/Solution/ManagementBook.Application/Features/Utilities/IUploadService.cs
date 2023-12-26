namespace ManagementBook.Application.Features.Utilities;

using LanguageExt;
using LanguageExt.Common;

public record FileSend
{
    public required byte[] Data { get; init; }
    public required Guid BookId { get; init; }
}

public interface IUploadService
{
    Task<Result<Unit>> Send(FileSend file, EventHandler<EventArgs> progresEvent);
}
