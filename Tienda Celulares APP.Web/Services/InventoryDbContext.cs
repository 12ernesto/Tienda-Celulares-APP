using Microsoft.EntityFrameworkCore;

namespace Tienda_Celulares_APP.Web.Services;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<MobilePhone> MobilePhones { get; set; } = null!;
    public DbSet<Brand> Brands { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Supplier> Suppliers { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<StockMovement> StockMovements { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<MobilePhone>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.Marca).HasMaxLength(100);
            eb.Property(e => e.Modelo).HasMaxLength(100);
            eb.Property(e => e.Color).HasMaxLength(50);
            eb.Property(e => e.Precio).HasColumnType("decimal(18,2)");
            eb.Property(e => e.SKU).HasMaxLength(50);
            eb.Property(e => e.IMEI).HasMaxLength(50);
            eb.HasOne(e => e.Brand).WithMany(b => b.MobilePhones).HasForeignKey(e => e.BrandId).OnDelete(DeleteBehavior.SetNull);
            eb.HasOne(e => e.Category).WithMany(c => c.MobilePhones).HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.SetNull);
            eb.HasOne(e => e.Supplier).WithMany(s => s.MobilePhones).HasForeignKey(e => e.SupplierId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Brand>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Category>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Supplier>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.Name).HasMaxLength(200);
            eb.Property(e => e.ContactEmail).HasMaxLength(200);
            eb.Property(e => e.ContactPhone).HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.CustomerName).HasMaxLength(200);
            eb.Property(e => e.CustomerEmail).HasMaxLength(200);
            eb.HasMany(e => e.Items).WithOne(i => i.Order).HasForeignKey(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
            eb.HasOne(e => e.MobilePhone).WithMany().HasForeignKey(e => e.MobilePhoneId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StockMovement>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.MovementType).HasMaxLength(50);
            eb.HasOne(e => e.MobilePhone).WithMany().HasForeignKey(e => e.MobilePhoneId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
