using Aquantica.BLL.Services;
using Aquantica.DAL.Repositories;
using Aquantica.DAL.Seeder;
using Aquantica.DAL.UnitOfWork;
using Autofac;

namespace Aquantica.API;

public class DiModule : Module
{
    private readonly IConfiguration _configuration;

    public DiModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        
        RegisterServices(builder);

        builder.RegisterType<Seeder>()
            .As<ISeeder>()
            .SingleInstance();

        builder.RegisterType<CustomUserManager>().InstancePerLifetimeScope();
    }

    private void RegisterServices(ContainerBuilder builder)
    {
        var bllAssembly = typeof(BLL.AssemblyRunner).Assembly;
        builder.RegisterAssemblyTypes(bllAssembly)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerDependency();
    }
    
}