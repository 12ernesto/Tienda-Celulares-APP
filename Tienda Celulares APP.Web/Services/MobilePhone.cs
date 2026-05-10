namespace Tienda_Celulares_APP.Web.Services;

using System.ComponentModel.DataAnnotations;

public class MobilePhone
{
    public Guid Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Marca { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Modelo { get; set; } = string.Empty;

    [StringLength(50)]
    public string Color { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Range(0, 1000000)]
    public decimal Precio { get; set; }
    // Optional relations for a richer DB model
    public Guid? BrandId { get; set; }
    public Brand? Brand { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public string? SKU { get; set; }
    public string? IMEI { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
