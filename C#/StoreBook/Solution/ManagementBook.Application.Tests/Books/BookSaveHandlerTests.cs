namespace ManagementBook.Application.Tests.Books;

using AutoMapper;
using FluentAssertions;
using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Application.Features.Books.Commands;
using ManagementBook.Application.Features.Books.Handlers;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Cross.Errors;
using Moq;

public class BookSaveHandlerTests
{
    Mock<IMapper> _mockMapper;
    Mock<IBookRepository> _mockRepository;
    BookSaveHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockMapper = new();
        _mockRepository = new();
        _handler = new(_mockMapper.Object, _mockRepository.Object);
    }

    [Test]
    public async Task BookSaveHandlerTestss_Handle_Save_ShouldBeOk()
    {
        //arrange
        var id = Guid.NewGuid();
        CancellationTokenSource cancellationTokenSource = new();
        BookSaveCommand bookByIdQuery = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            Released = DateTime.Now,
        };

        Book book = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };

        _mockMapper.Setup(m=> m.Map<Book>(bookByIdQuery))
                   .Returns(book)
                   .Verifiable();

        _mockRepository.Setup(r => r.Save(It.IsAny<Option<Book>>()))
                       .ReturnsAsync(new Result<Unit>(Unit.Default))
                       .Verifiable();

        //action
        var result = await _handler.Handle(bookByIdQuery, cancellationTokenSource.Token);

        //verifies
        result.IsSuccess.Should().BeTrue();

        result.IfSucc(_ =>
        {
            _mockMapper.Verify();
            _mockRepository.Verify();
            _mockRepository.VerifyNoOtherCalls();
        });
    }

    [Test]
    public async Task BookSaveHandlerTestss_Handle_Save_MapperThrowsAutoMapperMappingException_ButReturnInternalError()
    {
        //arrange
        var id = Guid.NewGuid();
        CancellationTokenSource cancellationTokenSource = new();
        BookSaveCommand bookByIdQuery = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            Released = DateTime.Now,
        };

        _mockMapper.Setup(m => m.Map<Book>(bookByIdQuery))
                   .Throws<AutoMapperMappingException>()
                   .Verifiable();

        //action
        var result = await _handler.Handle(bookByIdQuery, cancellationTokenSource.Token);

        //verifies
        result.IsFaulted.Should().BeTrue();

        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InternalError>();
            _mockMapper.Verify();
            _mockRepository.Verify();
            _mockRepository.VerifyNoOtherCalls();
        });
    }
}
