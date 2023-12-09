using Aquantica.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aquantica.DAL;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<AccessAction> AccessActions { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<Settings> Settings { get; set; }

    public DbSet<IrrigationEvent> IrrigationHistory { get; set; }

    public DbSet<IrrigationSection> IrrigationSections { get; set; }

    public DbSet<IrrigationSectionType> SectionTypes { get; set; }

    public DbSet<Location> Locations { get; set; }

    public DbSet<IrrigationRuleset> IrrigationRuleSets { get; set; }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    public DbSet<WeatherRecord> WeatherRecords { get; set; }

    public DbSet<BackgroundJob> BackgroundJobs { get; set; }

    public DbSet<BackgroundJobEvent> BackgroundJobEvents { get; set; }

    public DbSet<MenuItem> MenuItems { get; set; }


    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).DateUpdated = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).DateCreated = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).DateUpdated = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).DateCreated = DateTime.Now;
            }
        }

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UpdateStructure(modelBuilder);
        //SeedData(modelBuilder);
    }

    private void UpdateStructure(ModelBuilder modelBuilder)
    {
        //User
        modelBuilder.Entity<RefreshToken>()
            .HasOne(opt => opt.User)
            .WithOne(x => x.RefreshToken)
            .HasForeignKey<RefreshToken>(token => token.UserId);

        modelBuilder.Entity<User>()
            .HasOne(opt => opt.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(user => user.RoleId);

        modelBuilder.Entity<Role>()
            .HasMany(opt => opt.AccessActions)
            .WithMany(x => x.Roles)
            .UsingEntity<Dictionary<string, object>>(
                "RoleAccessAction",
                opt => opt.HasOne<AccessAction>().WithMany().HasForeignKey("AccessActionId"),
                opt => opt.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                opt =>
                {
                    opt.HasKey("AccessActionId", "RoleId");
                    opt.ToTable("RoleAccessAction");
                }
            );


        //Sections and rulesets
        modelBuilder.Entity<Settings>();

        modelBuilder.Entity<IrrigationEvent>()
            .HasOne(x => x.Section)
            .WithMany(x => x.IrrigationEvents)
            .HasForeignKey(x => x.SectionId);

        modelBuilder.Entity<IrrigationSection>()
            .HasOne(x => x.ParentSection)
            .WithMany()
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<IrrigationSection>()
            .HasOne(x => x.IrrigationRuleset)
            .WithMany()
            .HasForeignKey(x => x.SectionRulesetId);

        modelBuilder.Entity<IrrigationSection>()
            .HasOne(x => x.IrrigationSectionType)
            .WithMany(x => x.IrrigationSections)
            .HasForeignKey(x => x.IrrigationSectionTypeId);

        modelBuilder.Entity<IrrigationSection>()
            .HasOne(x => x.Location)
            .WithMany()
            .HasForeignKey(x => x.LocationId);

        modelBuilder.Entity<IrrigationRuleset>()
            .HasMany(x => x.IrrigationSections)
            .WithOne(x => x.IrrigationRuleset)
            .HasForeignKey(x => x.SectionRulesetId);

        modelBuilder.Entity<WeatherForecast>()
            .HasOne(x => x.Location)
            .WithMany(x => x.WeatherForecasts)
            .HasForeignKey(x => x.LocationId);

        modelBuilder.Entity<WeatherRecord>()
            .HasMany(x => x.WeatherForecasts)
            .WithOne(x => x.WeatherRecord)
            .HasForeignKey(x => x.WeatherRecordId);

        modelBuilder.Entity<BackgroundJob>()
            .HasOne(x => x.IrrigationSection)
            .WithMany(x => x.BackgroundJobs)
            .HasForeignKey(x => x.IrrigationSectionId);

        modelBuilder.Entity<BackgroundJobEvent>()
            .HasOne(x => x.BackgroundJob)
            .WithMany(x => x.BackgroundJobEvents)
            .HasForeignKey(x => x.BackgroundJobId);

        modelBuilder.Entity<MenuItem>()
            .HasOne(x => x.ParentMenuItem)
            .WithMany()
            .HasForeignKey(x => x.ParentId);

        modelBuilder.Entity<MenuItem>()
            .HasOne(x => x.AccessAction)
            .WithMany()
            .HasForeignKey(x => x.AccessActionId);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessAction>().HasData(
            new AccessAction { Id = 1, Name = "ViewUsers" },
            new AccessAction { Id = 2, Name = "EditUsers" },
            new AccessAction { Id = 3, Name = "ViewRoles" },
            new AccessAction { Id = 4, Name = "EditRoles" },
            new AccessAction { Id = 5, Name = "ViewSettings" },
            new AccessAction { Id = 6, Name = "EditSettings" },
            new AccessAction { Id = 7, Name = "ViewIrrigationHistory" },
            new AccessAction { Id = 8, Name = "EditIrrigationHistory" }
        );

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" }
        );
    }
}