﻿namespace ManagementBook.Api.Modules;
using Autofac;
using ManagementBook.Api.Behaviors;
using ManagementBook.Application;
using MediatR;
using System.Reflection;
using Module = Autofac.Module;

public class MediatRModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = typeof(ApplicationAssembly).GetTypeInfo().Assembly;

        builder.RegisterAssemblyTypes(assembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(IRequestHandler<>))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(INotificationHandler<>))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(LoggingBehavior<,>))
               .As(typeof(IPipelineBehavior<,>));

        builder.RegisterGeneric(typeof(ValidatorBehavior<,>))
               .As(typeof(IPipelineBehavior<,>));

    }
}