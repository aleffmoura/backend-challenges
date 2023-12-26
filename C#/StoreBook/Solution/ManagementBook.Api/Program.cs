using Autofac;
using Autofac.Extensions.DependencyInjection;
using ManagementBook.Api.Behaviors;
using ManagementBook.Api.Endpoints;
using ManagementBook.Api.Handlers;
using ManagementBook.Api.Modules;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddEndpointsApiExplorer()
       .AddControllers()
       .AddNewtonsoftJson(op =>
       {
           op.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
           op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
       });

builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<RemoveAntiforgeryHeaderOperationFilter>();
});

builder.Services.AddMvc(options => options.Filters.Add(new ErrorHandlerAttribute()));

builder.Services.Configure<RequestLocalizationOptions>(op =>
{
    op.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR");
});

builder.Host
       .UseServiceProviderFactory(new AutofacServiceProviderFactory())
       .ConfigureContainer<ContainerBuilder>(containerBuilder =>
       {
           var configuration = MediatRConfigurationBuilder
                               .Create(typeof(Program).Assembly)
                               .WithAllOpenGenericHandlerTypesRegistered()
                               .WithRegistrationScope(RegistrationScope.Scoped)
                               .Build();

           containerBuilder.RegisterModule(new FluentValidationModule());
           containerBuilder.RegisterModule(new GlobalModule<Program>(builder.Configuration));
           containerBuilder.RegisterModule(new AmazonModule(builder.Configuration));
           containerBuilder.RegisterModule(new MediatRModule());

           containerBuilder.RegisterMediatR(configuration);
       })
    .ConfigureHostOptions(o =>
    {
        o.ShutdownTimeout = TimeSpan.FromSeconds(60);
    });

var app = builder.Build();
app.UseAntiforgery();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Book endpoints
app.BookGetEndpoint()
   .BookGetByIdEndpoint()
   .BookPostEndpoint()
   .BookPutEndpoint()
   .BookPatchEndpoint();

app.Run();
