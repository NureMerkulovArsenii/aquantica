using Aquantica.Core.Entities;
using Aquantica.Core.Enums;
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
    
    public DbSet<IrrigationRuleset> IrrigationRuleSets { get; set; }
    
    public DbSet<IrrigationSectionType> SectionTypes { get; set; }
    

    public override int SaveChanges()
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

        return base.SaveChanges();
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
                    opt.ToTable("RoleAccessActions");
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
            .WithMany()
            .HasForeignKey(x => x.SectionTypeId);
        
        modelBuilder.Entity<IrrigationRuleset>()
            .HasMany(x => x.IrrigationSections)
            .WithOne(x => x.IrrigationRuleset)
            .HasForeignKey(x => x.SectionRulesetId);
        
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

        modelBuilder.Entity<Settings>().HasData(
            new Settings { Id = 1, Name = "IrrigationDuration", Value = "10", ValueType = SettingValueType.Number },
            new Settings { Id = 2, Name = "IrrigationInterval", Value = "1", ValueType = SettingValueType.Number },
            new Settings { Id = 3, Name = "IrrigationStartTime", Value = "10:00", ValueType = SettingValueType.String },
            new Settings { Id = 4, Name = "IrrigationEndTime", Value = "20:00", ValueType = SettingValueType.String },
            new Settings { Id = 5, Name = "IrrigationEnabled", Value = "true", ValueType = SettingValueType.Boolean },
            new Settings
                { Id = 6, Name = "IrrigationWaterConsumption", Value = "0.5", ValueType = SettingValueType.Number }
        );

        modelBuilder.Entity<IrrigationSection>().HasData(
            new IrrigationSection { Id = 1, Name = "Section 1", Number = 1 },
            new IrrigationSection { Id = 2, Name = "Section 2", Number = 2 },
            new IrrigationSection { Id = 3, Name = "Section 3", Number = 3 },
            new IrrigationSection { Id = 4, Name = "Section 4", Number = 4 },
            new IrrigationSection { Id = 5, Name = "Section 5", Number = 5 },
            new IrrigationSection { Id = 6, Name = "Section 6", Number = 6 },
            new IrrigationSection { Id = 7, Name = "Section 7", Number = 7 },
            new IrrigationSection { Id = 8, Name = "Section 8", Number = 8 },
            new IrrigationSection { Id = 9, Name = "Section 9", Number = 9 }
        );
    }
}