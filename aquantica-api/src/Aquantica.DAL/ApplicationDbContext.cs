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
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UpdateStructure(modelBuilder);
        SeedData(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void UpdateStructure(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<User>().HasKey(x => x.Id);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {

    }

}
