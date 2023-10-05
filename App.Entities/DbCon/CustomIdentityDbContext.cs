using App.Entities.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Entities.DbCon;

public class CustomIdentityDbContext : IdentityDbContext<CustomIdentityUser, CustomIdentityRole, string>
{
    public DbSet<Repairer>? Repairer { get; set; }
    public DbSet<Category>? Category { get; set; }
    public DbSet<Message>? Messages { get; set; }
    public DbSet<Works>? Works { get; set; }

    public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> opt) : base(opt) { }
    public CustomIdentityDbContext() { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Category>()
            .HasMany(_ => _.Repairers)
            .WithOne(_ => _.Category)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Message>()
            .HasOne(_ => _.Sender)
            .WithMany(_ => _.Messages)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Repairer>()
            .HasOne(_ => _.User)
            .WithMany(_ => _.Repairers)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Category>(_ => _.HasIndex(_ => _.Name).IsUnique());
        
        base.OnModelCreating(builder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            optionsBuilder.UseSqlServer(builder.Configuration["ConnectionStrings:default"]);
        }
    }
}
