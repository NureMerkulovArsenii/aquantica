using Aquantica.DAL;
using Aquantica.DAL.Repositories;
using Aquantica.DAL.Seeder;
using Aquantica.DAL.UnitOfWork;
using Autofac;
using Hangfire;
using Hangfire.Autofac;
using Microsoft.EntityFrameworkCore;

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
        // RegisterRepositories(builder);
        // RegisterUnitOfWork(builder);

        // builder.RegisterType<ApplicationDbContext>().WithParameter(
        //         (info, context) => info.ParameterType == typeof(DbContextOptions<ApplicationDbContext>),
        //         (info, context) => new DbContextOptionsBuilder<ApplicationDbContext>()
        //             .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
        //             .Options)
        //     .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        // builder.RegisterType(typeof(UnitOfWork))
        //     .As(typeof(IUnitOfWork));

        RegisterServices(builder);

        builder.RegisterType<Seeder>()
            .As<ISeeder>()
            .SingleInstance();
    }

    private void RegisterServices(ContainerBuilder builder)
    {
        // Register services here
        var bllAssembly = typeof(BLL.AssemblyRunner).Assembly;
        builder.RegisterAssemblyTypes(bllAssembly)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerDependency();
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