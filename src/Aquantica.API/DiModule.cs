using Autofac;

namespace Aquantica.API;

public class DiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterServices(builder);
        RegisterRepositories(builder);
        RegisterUnitOfWork(builder);        
    }

    private void RegisterServices(ContainerBuilder builder)
    {
        // Register services here
        var servicesAssembly = typeof(Infrastructure.AssemblyRunner).Assembly;
        builder.RegisterAssemblyTypes(servicesAssembly)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }

    private void RegisterRepositories(ContainerBuilder builder)
    {
        // Register repositories here
        var infrastructureAssembly = typeof(Infrastructure.AssemblyRunner).Assembly;

        builder.RegisterAssemblyTypes(infrastructureAssembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }

    private void RegisterUnitOfWork(ContainerBuilder builder)
    {
        // Register unit of work here
        var infrastructureAssembly = typeof(Infrastructure.AssemblyRunner).Assembly;
        builder.RegisterAssemblyTypes(infrastructureAssembly)
            .Where(t => t.Name.EndsWith("UnitOfWork"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
