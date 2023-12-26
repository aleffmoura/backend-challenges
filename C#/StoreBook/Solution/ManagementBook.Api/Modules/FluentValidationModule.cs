namespace ManagementBook.Api.Modules;

using Autofac;
using FluentValidation;
using ManagementBook.Application;

public class FluentValidationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(ApplicationAssembly).Assembly)
            .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}