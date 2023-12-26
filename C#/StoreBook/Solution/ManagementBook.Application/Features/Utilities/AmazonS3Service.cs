namespace ManagementBook.Application.Features.Utilities;

using Amazon.S3;
using Amazon.S3.Transfer;
using LanguageExt;
using LanguageExt.Common;
using ManagementBook.Infra.Cross.Errors;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

public class AmazonS3Service : IUploadService
{
    private ITransferUtility _transferUtility;
    private IConfigurationRoot _configuration;

    public AmazonS3Service(ITransferUtility transferUtility, IConfigurationRoot configuration)
    {
        _transferUtility = transferUtility;
        _configuration = configuration;
    }

    public async Task<Result<Unit>> Send(FileSend send, EventHandler<EventArgs> progresEvent)
    {
        try
        {
            var stream = new MemoryStream(send.Data);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                BucketName = _configuration["amazon:s3:bucketName"],
                InputStream = stream,
                Key = $"{send.BookId}.png",
            };

            if (bool.Parse(_configuration["amazon:s3:publicRead"] ?? "false"))
                uploadRequest.CannedACL = S3CannedACL.PublicRead;

            EventHandler<UploadProgressArgs> eventProgress = (o, e) => { progresEvent(o, e); };

            uploadRequest.UploadProgressEvent += eventProgress;

            await _transferUtility.UploadAsync(uploadRequest);

            uploadRequest.UploadProgressEvent -= eventProgress;

            return Unit.Default;
        }
        catch (AmazonS3Exception e)
        {
            return new Result<Unit>(new InternalError($"Erro ao fazer upload para o Amazon S3: {e.Message}"));
        }
        catch (Exception e)
        {
            return new Result<Unit>(new InternalError($"Erro desconhecido: {e.Message}"));
        }
    }
}
