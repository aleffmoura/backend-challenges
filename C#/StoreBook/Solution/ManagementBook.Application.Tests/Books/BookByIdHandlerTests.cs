namespace ManagementBook.Application.Tests.Books;

using FluentAssertions;
using LanguageExt.Common;
using ManagementBook.Application.Features.Books.Handlers;
using ManagementBook.Application.Features.Books.Queries;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Cross.Errors;
using Moq;

public class BookByIdHandlerTests
{
    Mock<IBookRepository> _mockRepository;
    BookByIdHandler _handler;
    [SetUp]
    public void SetUp()
    {
        _mockRepository = new();
        _handler = new(_mockRepository.Object);
    }

    [Test]
    public async Task BookByIdQueryHandlerTests_Handle_ExistsBook_ShouldBeOk()
    {
        //arrange
        var id = Guid.NewGuid();
        CancellationTokenSource cancellationTokenSource = new();
        BookByIdQuery bookByIdQuery = new(id);
        Book book = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };

        Result<Book> response = new(book);

        _mockRepository.Setup(r => r.GetById(id))
                       .ReturnsAsync(response)
                       .Verifiable();

        //action
        var result = await _handler.Handle(bookByIdQuery, cancellationTokenSource.Token);

        //verifies
        result.IsSuccess.Should().BeTrue();
        result.IfSucc(book =>
        {
            book.Id.Should().Be(id);
            _mockRepository.Verify();
            _mockRepository.VerifyNoOtherCalls();
        });
    }

    [Test]
    public async Task BookByIdQueryHandlerTests_Handle_BookNotFound_ShouldBeError()
    {
        //arrange
        var id = Guid.NewGuid();
        CancellationTokenSource cancellationTokenSource = new();
        BookByIdQuery bookByIdQuery = new(id);

        Result<Book> response = new(new NotFoundError($"Book with {{id}}: {id} not found."));

        _mockRepository.Setup(r => r.GetById(id))
                       .ReturnsAsync(response)
                       .Verifiable();

        //action
        var result = await _handler.Handle(bookByIdQuery, cancellationTokenSource.Token);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<NotFoundError>();
            _mockRepository.Verify();
            _mockRepository.VerifyNoOtherCalls();
        });
    }
}
