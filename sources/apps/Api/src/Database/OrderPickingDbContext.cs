using Api.Orders;
using Microsoft.EntityFrameworkCore;

namespace Api.Database;

public class OrderPickingDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public OrderPickingDbContext(DbContextOptions<OrderPickingDbContext> options) : base(options)
    {
    }
}