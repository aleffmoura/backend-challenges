namespace ManagementBook.Application.Tests.Books;

using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Application.Features.Books.Commands;
using ManagementBook.Application.Features.Books.Handlers;
using ManagementBook.Application.Features.Books.Queries;
using ManagementBook.Domain.Books;
using static LanguageExt.Prelude;

using Moq;
using FluentAssertions;
using ManagementBook.Infra.Cross.Errors;

public class BookRemoveHandlerTests
{
    Mock<IBookRepository> _mockRepository;
    BookRemoveHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new();
        _handler = new(_mockRepository.Object);
    }

    [Test]
    public async Task BookRemoveHandlerTests_Handle_Remove_ShouldBeOk()
    {
        //arrange
        var id = Guid.NewGuid();
        CancellationTokenSource cancellationTokenSource = new();
        BookRemoveCommand removeCommand = new(id);
        var response = new Result<Unit>(unit);
        _mockRepository.Setup(s => s.Remove(id))
                       .ReturnsAsync(response)
                       .Verifiable();

        //action
        var result = await _handler.Handle(removeCommand, cancellationTokenSource.Token);

        //verifies
        _mockRepository.Verify();
        result.IsSuccess.Should().BeTrue();
        result.IfSucc(_ =>
        {
            _mockRepository.Verify();
        });
    }

    [Test]
    public async Task BookRemoveHandlerTests_Handle_Remove_ReturnsNotFound_ShouldBeOk()
    {
        //arrange
        var id = Guid.NewGuid();
        CancellationTokenSource cancellationTokenSource = new();
        BookRemoveCommand removeCommand = new(id);
        var response = new Result<Unit>(new NotFoundError("Error"));
        _mockRepository.Setup(s => s.Remove(id))
                       .ReturnsAsync(response)
                       .Verifiable();

        //action
        var result = await _handler.Handle(removeCommand, cancellationTokenSource.Token);

        //verifies
        _mockRepository.Verify();
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<NotFoundError>();
            _mockRepository.Verify();
        });
    }
}
