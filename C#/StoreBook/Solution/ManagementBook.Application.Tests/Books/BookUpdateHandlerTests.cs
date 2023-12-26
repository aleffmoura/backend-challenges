namespace ManagementBook.Application.Tests.Books;
using AutoMapper;
using LanguageExt.Common;
using LanguageExt;
using ManagementBook.Application.Features.Books.Commands;
using ManagementBook.Application.Features.Books.Handlers;
using ManagementBook.Domain.Books;

using Moq;
using FluentAssertions;
using ManagementBook.Infra.Cross.Errors;

public class BookUpdateHandlerTests
{
    Mock<IMapper> _mockMapper;
    Mock<IBookRepository> _mockRepository;
    BookUpdateHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockMapper = new();
        _mockRepository = new();
        _handler = new(_mockMapper.Object, _mockRepository.Object);
    }

    [Test]
    public async Task BookUpdateHandlerTests_Handle_Update_ShouldBeOk()
    {
        //arrange
        var id = Guid.NewGuid();
        CancellationTokenSource cancellationTokenSource = new();
        BookUpdateCommand bookByIdQuery = new()
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

        _mockMapper.Setup(m => m.Map<Book>(bookByIdQuery))
                   .Returns(book)
                   .Verifiable();

        _mockRepository.Setup(r => r.Update(It.IsAny<Option<Book>>()))
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
    public async Task BookUpdateHandlerTests_Handle_Update_MapThrowException_ButReturnIsInternalError()
    {
        //arrange
        var id = Guid.NewGuid();
        CancellationTokenSource cancellationTokenSource = new();
        BookUpdateCommand bookByIdQuery = new()
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
    [Test]
    public async Task BookUpdateHandlerTests_Handle_Update_ReturnException()
    {
        //arrange
        var id = Guid.NewGuid();
        CancellationTokenSource cancellationTokenSource = new();
        BookUpdateCommand bookByIdQuery = new()
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

        _mockMapper.Setup(m => m.Map<Book>(bookByIdQuery))
                   .Returns(book)
                   .Verifiable();

        _mockRepository.Setup(r => r.Update(It.IsAny<Option<Book>>()))
                       .ReturnsAsync(new Result<Unit>(new NotFoundError("Test")))
                       .Verifiable();
        //action
        var result = await _handler.Handle(bookByIdQuery, cancellationTokenSource.Token);

        //verifies
        result.IsFaulted.Should().BeTrue();

        result.IfFail(fail =>
        {
            fail.Should().BeOfType<NotFoundError>();
            _mockMapper.Verify();
            _mockRepository.Verify();
            _mockRepository.VerifyNoOtherCalls();
        });
    }
}
