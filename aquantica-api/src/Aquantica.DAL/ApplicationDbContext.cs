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
    
    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            ((BaseEntity) entityEntry.Entity).DateUpdated = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity) entityEntry.Entity).DateCreated = DateTime.Now;
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
        SeedData(modelBuilder);
    }

    private void UpdateStructure(ModelBuilder modelBuilder)
    {
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
    }

    private void SeedData(ModelBuilder modelBuilder)
    {

    }

}
