using EffectiveMobileTest.Models;
using Microsoft.EntityFrameworkCore;

namespace EffectiveMobileTest.Context;

public class DeliveryDbContext:DbContext
{
    public DbSet<Order> Orders { get; set; }

    public DeliveryDbContext(DbContextOptions<DeliveryDbContext>options):base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().ToTable("Orders");
    }
}