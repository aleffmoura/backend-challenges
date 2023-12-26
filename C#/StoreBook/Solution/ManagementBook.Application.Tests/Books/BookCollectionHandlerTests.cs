namespace ManagementBook.Application.Tests.Books;

using FluentAssertions;
using ManagementBook.Application.Features.Books.Handlers;
using ManagementBook.Application.Features.Books.Queries;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Cross.Errors;
using Moq;
using System.Data.SqlTypes;

[TestFixture]
public class BookCollectionHandlerTests
{
    Mock<IBookRepository> _mockRepository;
    BookCollectionHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new();
        _handler = new(_mockRepository.Object);
    }

    [Test]
    public async Task BookCollectionHandlerTests_Handle_BookCollectionQuery_ShouldBeSuccess()
    {
        //arrange
        CancellationTokenSource cancellationTokenSource = new();
        BookCollectionQuery bookCollectionQuery = new();
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };
        List<Book> books = [
            book
        ];
        int quantityBooks = 1;

        _mockRepository.Setup(r => r.GetAll())
                       .ReturnsAsync(books.AsQueryable())
                       .Verifiable();

        //action
        var result = await _handler.Handle(bookCollectionQuery, cancellationTokenSource.Token);

        //verifies
        result.IsSuccess.Should().BeTrue();
        result.IfSucc(books =>
        {
            books.Should().NotBeNullOrEmpty();
            books.Count().Should().Be(expected: quantityBooks);
            _mockRepository.Verify();
            _mockRepository.VerifyNoOtherCalls();
        });
    }

    [Test]
    public async Task BookCollectionHandlerTests_Handle_BookCollectionQuery_ShouldBeSqlTruncateException_ButReturnIsInternalError()
    {
        //arrange
        CancellationTokenSource cancellationTokenSource = new();
        BookCollectionQuery bookCollectionQuery = new();
       
        _mockRepository.Setup(r => r.GetAll())
                       .Throws<SqlTruncateException>()
                       .Verifiable();

        //action
        var result = await _handler.Handle(bookCollectionQuery, cancellationTokenSource.Token);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InternalError>();
            _mockRepository.Verify();
            _mockRepository.VerifyNoOtherCalls();
        });
    }
}
