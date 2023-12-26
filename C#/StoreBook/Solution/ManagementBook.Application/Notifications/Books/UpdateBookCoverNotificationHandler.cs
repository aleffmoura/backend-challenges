namespace ManagementBook.Application.Notifications.Books;

using Amazon.S3.Transfer;
using ManagementBook.Application.Features.Utilities;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Cross.Errors;
using ManagementBook.Infra.Data.Features.Books;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdateBookCoverNotificationHandler : INotificationHandler<BookCoverNotification>
{
    private IServiceProvider _provider;

    public UpdateBookCoverNotificationHandler(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task Handle(BookCoverNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            var scoped = _provider.CreateScope();
            var uploadService = scoped.ServiceProvider.GetService<IUploadService>();
            var bookRepository = scoped.ServiceProvider.GetService<IBookRepository>();
            var configuration = scoped.ServiceProvider.GetService<IConfigurationRoot>();

            if (uploadService is null || bookRepository is null || configuration is null)
            {
                Console.WriteLine("reference not fount for uploadService or bookRepository or configuration");
                return;
            }

            await uploadService.Send(new FileSend
            {
                BookId = notification.BookId,
                Data = notification.Data,
            }, async (o, e) =>
            {
                if (e is UploadProgressArgs { PercentDone: 100 } args)
                {
                    var book = await bookRepository.GetById(notification.BookId);
                    var url = $"https://{configuration["amazon:s3:bucketName"]}.s3.{configuration["amazon:s3:region"]}.amazonaws.com/{notification.BookId}.png";
                    book.IfSucc(async b => await bookRepository.Update(b with { BookCoverUrl = url }));
                }
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
