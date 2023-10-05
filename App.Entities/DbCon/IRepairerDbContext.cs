using App.Entities.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace App.Entities.DbCon;

public class IRepairerDbContext : DbContext
{
    public DbSet<Repairer>? Repairer { get; set; }
    public DbSet<Category>? Category { get; set; }

    public IRepairerDbContext() { }
    public IRepairerDbContext(DbContextOptions<IRepairerDbContext> optionsBuilder) : base(optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasMany(_ => _.Repairers)
            .WithOne(_ => _.Category)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Category>(_ => _.HasIndex(_ => _.Name).IsUnique());
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
