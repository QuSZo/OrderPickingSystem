using Api.Orders;
using Api.TravelingSalesmanAlgorithms;
using Microsoft.EntityFrameworkCore;

namespace Api.Database;

public class OrderPickingDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public OrderPickingDbContext(DbContextOptions<OrderPickingDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.TspAlgorithmResults)
            .WithOne()
            .HasForeignKey<TspAlgorithmResult>("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TspAlgorithmResult>()
            .HasMany(t => t.Path)
            .WithOne()
            .HasForeignKey("TspAlgorithmResultId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}