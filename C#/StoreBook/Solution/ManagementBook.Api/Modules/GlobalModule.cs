namespace ManagementBook.Api.Modules;
using Autofac;

using AutoMapper;
using ManagementBook.Application;
using ManagementBook.Application.Features.Utilities;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Data.Base;
using ManagementBook.Infra.Data.Features.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Module = Autofac.Module;
public class GlobalModule<TProgram> : Module
{
    IConfigurationRoot Configuration { get; }
    public GlobalModule(IConfigurationRoot configuration)
    {
        Configuration = configuration;
    }
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(context =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookStoreContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("BookStore"), o => o.CommandTimeout(commandTimeout: 10));
            return new BookStoreContext(optionsBuilder.Options);
        }).InstancePerLifetimeScope();

        builder.RegisterType<BookRepository>()
               .As<IBookRepository>()
               .InstancePerLifetimeScope();

        builder.Register(_ => Configuration)
               .As<IConfigurationRoot>()
               .InstancePerLifetimeScope();
        
        builder.Register(ctx =>
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(TProgram));
                cfg.AddMaps(typeof(ApplicationAssembly));
            });

            return configuration.CreateMapper();
        }).As<IMapper>()
          .InstancePerLifetimeScope();
    }
}