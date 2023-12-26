namespace ManagementBook.Api.Mappers;

using AutoMapper;
using ManagementBook.Api.DTOs;
using ManagementBook.Api.ViewModels;
using ManagementBook.Application.Features.Books.Commands;
using ManagementBook.Domain.Books;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookCreateDto, BookSaveCommand>()
            .ForMember(ds => ds.Id, m => m.MapFrom(src => Guid.NewGuid()))
            .ForMember(ds => ds.Released, m => m.MapFrom(src => src.ReleaseData));

        CreateMap<BookSaveCommand, Book>()
            .ForMember(ds => ds.Id, m => m.MapFrom(src => src.Id))
            .ForMember(ds => ds.ReleaseDate, m => m.MapFrom(src => src.Released));

        CreateMap<(Guid id, BookUpdateDto dto), BookUpdateCommand>()
            .ForMember(ds => ds.Id, m => m.MapFrom(src => src.id))
            .ForMember(ds => ds.Author, m => m.MapFrom(src => src.dto.Author))
            .ForMember(ds => ds.Title, m => m.MapFrom(src => src.dto.Title))
            .ForMember(ds => ds.Released, m => m.MapFrom(src => src.dto.ReleaseData));

        CreateMap<BookUpdateCommand, Book>()
            .ForMember(ds => ds.Id, m => m.MapFrom(src => src.Id))
            .ForMember(ds => ds.ReleaseDate, m => m.MapFrom(src => src.Released));

        CreateMap<Book, BookDetailViewModel>()
            .ForMember(ds => ds.Guid, m => m.MapFrom(src => src.Id))
            .ForMember(ds => ds.Author, m => m.MapFrom(src => src.Author))
            .ForMember(ds => ds.Title, m => m.MapFrom(src => src.Title))
            .ForMember(ds => ds.ReleaseDate, m => m.MapFrom(src => src.ReleaseDate.ToString("dd/MM/yyyy")));

        CreateMap<Book, BookResumeViewModel>()
            .ForMember(ds => ds.Guid, m => m.MapFrom(src => src.Id))
            .ForMember(ds => ds.Author, m => m.MapFrom(src => src.Author))
            .ForMember(ds => ds.Title, m => m.MapFrom(src => src.Title))
            .ForMember(ds => ds.ReleaseDate, m => m.MapFrom(src => src.ReleaseDate.ToString("dd/MM/yyyy")));

        CreateMap<(Guid id, BookCoverPatchDto dto), BookPatchCommand>()
            .ForMember(ds => ds.BookId, m => m.MapFrom(src => src.id))
            .ForMember(ds => ds.Data, m => m.MapFrom(src => src.dto.Data));
    }
}
