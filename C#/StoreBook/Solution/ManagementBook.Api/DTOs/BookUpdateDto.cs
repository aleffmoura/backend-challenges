namespace ManagementBook.Api.DTOs;

public class BookUpdateDto
{
    public string? Author { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseData { get; set; }
}
