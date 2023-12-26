namespace ManagementBook.Application.Tests.AmazonS3Services;

using Amazon.S3.Transfer;
using FluentAssertions;
using ManagementBook.Application.Features.Utilities;
using ManagementBook.Application.Notifications.Books;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Data.Features.Books;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;

[TestFixture]
public class UpdateBookCoverNotificationHandlerTests
{
    private Mock<ITransferUtility> _transferUtility;
    private Mock<IConfigurationRoot> _mockConfiguration;
    private Mock<IBookRepository> _bookRepository;
    private Mock<IServiceProvider> _provider;

    private AmazonS3Service _amazonS3Service;
    private UpdateBookCoverNotificationHandler _handler;
    [SetUp]
    public void SetUp()
    {
        _provider = new();
        _transferUtility = new();
        _bookRepository = new();
        _mockConfiguration = new();
        _amazonS3Service = new(_transferUtility.Object, _mockConfiguration.Object);

        _provider.Setup(p => p.GetService(typeof(IUploadService))).Returns(_amazonS3Service).Verifiable();
        _provider.Setup(p => p.GetService(typeof(IBookRepository))).Returns(_bookRepository.Object).Verifiable();
        _provider.Setup(p => p.GetService(typeof(IConfigurationRoot))).Returns(_mockConfiguration.Object).Verifiable();

        var mockScoped = new Mock<IServiceScope>();
        mockScoped.SetupGet(s => s.ServiceProvider).Returns(_provider.Object);

        var factory = new Mock<IServiceScopeFactory>();
        factory.Setup(p => p.CreateScope()).Returns(mockScoped.Object).Verifiable();

        _provider.Setup(p => p.GetService(typeof(IServiceScopeFactory))).Returns(factory.Object).Verifiable();

        _handler = new(_provider.Object);
    }

    [Test]
    public async Task UpdateBookCoverNotificationHandlerTests_HandleNotification_ShouldBeSucess()
    {
        //arrange
        CancellationTokenSource cts = new();
        BookCoverNotification bookCoverNotification = new()
        {
            BookId = Guid.NewGuid(),
            Data = [1, 1, 1, 1, 1, 10, 1, 1, 2, 1, 2, 1,]
        };

        _transferUtility.Setup(s => s.UploadAsync(It.IsAny<TransferUtilityUploadRequest>(), It.IsAny<CancellationToken>())).Verifiable();

        //action
        var action = async () => await _handler.Handle(bookCoverNotification, cts.Token);

        //arrange
        await action.Should().NotThrowAsync();

        _mockConfiguration.Verify();
    }
}
