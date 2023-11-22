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
        var bllAssembly = typeof(BLL.AssemblyRunner).Assembly;
        builder.RegisterAssemblyTypes(bllAssembly)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        //register helpers
        builder.RegisterAssemblyTypes(bllAssembly)
            .Where(t => t.Name.EndsWith("Helper"))
            .AsSelf()
            .InstancePerLifetimeScope();
    }

    private void RegisterRepositories(ContainerBuilder builder)
    {
        // Register repositories here
        var infrastructureAssembly = typeof(DAL.AssemblyRunner).Assembly;

        builder.RegisterAssemblyTypes(infrastructureAssembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }

    private void RegisterUnitOfWork(ContainerBuilder builder)
    {
        // Register unit of work here
        var infrastructureAssembly = typeof(DAL.AssemblyRunner).Assembly;
        builder.RegisterAssemblyTypes(infrastructureAssembly)
            .Where(t => t.Name.EndsWith("UnitOfWork"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
