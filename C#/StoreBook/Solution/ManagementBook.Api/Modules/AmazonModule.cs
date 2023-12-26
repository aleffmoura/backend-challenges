namespace ManagementBook.Api.Modules;

using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Autofac;
using ManagementBook.Application.Features.Utilities;
using Module = Autofac.Module;

public class AmazonModule : Module
{
    IConfigurationRoot Configuration { get; }
    public AmazonModule(IConfigurationRoot configuration)
    {
        Configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var endpoint = RegionEndpoint.GetBySystemName(Configuration["amazon:s3:region"]);

        builder.Register(r => new AmazonS3Client(Configuration["amazon:s3:accessKey"], Configuration["amazon:s3:secretAccessKey"], endpoint))
               .As<IAmazonS3>()
               .InstancePerLifetimeScope();

        builder.Register(r => new TransferUtility(r.Resolve<IAmazonS3>()))
               .As<ITransferUtility>()
               .InstancePerLifetimeScope();

        builder.Register(r => new AmazonS3Service(r.Resolve<ITransferUtility>(), Configuration))
               .As<IUploadService>()
               .InstancePerLifetimeScope();
    }
}