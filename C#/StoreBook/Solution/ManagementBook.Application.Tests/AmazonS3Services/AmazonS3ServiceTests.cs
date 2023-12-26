namespace ManagementBook.Application.Tests.AmazonS3Services;

using Amazon.S3;
using Amazon.S3.Transfer;
using FluentAssertions;
using ManagementBook.Application.Features.Utilities;
using ManagementBook.Infra.Cross.Errors;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;

public class AmazonS3ServiceTests
{
    private Mock<IConfigurationRoot> _mockConfiguration;
    private Mock<ITransferUtility> _mockTransfer;
    private AmazonS3Service _service;

    [SetUp]
    public void SetUp()
    {
        _mockTransfer = new();
        _mockConfiguration = new();
        _service = new AmazonS3Service(_mockTransfer.Object, _mockConfiguration.Object);
    }

    [Test]
    public async Task AmazonS3ServiceTests_SendImage_ShouldBeOk()
    {
        //arrange
        var bucketName = "bucketName";

        _mockConfiguration.Setup(c => c["amazon:s3:bucketName"])
                          .Returns(bucketName)
                          .Verifiable();

        _mockConfiguration.Setup(c => c["amazon:s3:publicRead"])
                        .Verifiable();

        _mockTransfer.Setup(t => t.UploadAsync(It.IsAny<TransferUtilityUploadRequest>(), It.IsAny<CancellationToken>()))
                     .Verifiable();

        FileSend fileSend = new()
        {
            BookId = Guid.NewGuid(),
            Data = [1, 1, 1, 1, 0, 1]
        };

        //action 
        var result = await _service.Send(fileSend, (o, e) => { });

        //verifies
        result.IsSuccess.Should().BeTrue();
        result.IfSucc(_ =>
        {
            _mockTransfer.Verify();
            _mockConfiguration.Verify();
            _mockTransfer.VerifyNoOtherCalls();
            _mockConfiguration.VerifyNoOtherCalls();
        });
    }

    [Test]
    public async Task AmazonS3ServiceTests_SendImage_UploadAsyncThrowsException_ShouldBeInternalError()
    {
        //arrange
        var bucketName = "bucketName";

        _mockConfiguration.Setup(c => c["amazon:s3:bucketName"])
                          .Returns(bucketName)
                          .Verifiable();

        _mockConfiguration.Setup(c => c["amazon:s3:publicRead"])
                        .Verifiable();

        _mockTransfer.Setup(t => t.UploadAsync(It.IsAny<TransferUtilityUploadRequest>(), It.IsAny<CancellationToken>()))
                     .Throws(new AmazonS3Exception("Error"))
                     .Verifiable();

        FileSend fileSend = new()
        {
            BookId = Guid.NewGuid(),
            Data = [1, 1, 1, 1, 0, 1]
        };

        //action 
        var result = await _service.Send(fileSend, (o, e) => { });

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InternalError>();
            _mockTransfer.Verify();
            _mockConfiguration.Verify();
            _mockTransfer.VerifyNoOtherCalls();
            _mockConfiguration.VerifyNoOtherCalls();
        });
    }

    [Test]
    public async Task AmazonS3ServiceTests_SendImage_ConfigurationThrowsGenericException_ShouldBeInternalError()
    {
        //arrange
        _mockConfiguration.Setup(c => c["amazon:s3:bucketName"])
                          .Throws<ArgumentNullException>()
                          .Verifiable();
        
        FileSend fileSend = new()
        {
            BookId = Guid.NewGuid(),
            Data = [1, 1, 1, 1, 0, 1]
        };

        //action 
        var result = await _service.Send(fileSend, (o, e) => { });

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InternalError>();
            _mockConfiguration.Verify();
            _mockTransfer.VerifyNoOtherCalls();
            _mockConfiguration.VerifyNoOtherCalls();
        });
    }
}
