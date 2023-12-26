namespace ManagementBook.Api.ViewModels;

public record BookDetailViewModel
{
    public Guid Guid { get; set; } 
    public string Author  { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ReleaseDate { get; set; } = string.Empty;
    public string? BookCoverUrl { get; set; } = string.Empty;
}
