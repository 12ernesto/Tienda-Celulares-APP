namespace Tienda_Celulares_APP.Web.Services;

public class StockMovement
{
    public Guid Id { get; set; }
    public Guid MobilePhoneId { get; set; }
    public MobilePhone? MobilePhone { get; set; }
    public int Quantity { get; set; }
    public string MovementType { get; set; } = string.Empty; // e.g. Inbound, Outbound, Adjustment
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}
