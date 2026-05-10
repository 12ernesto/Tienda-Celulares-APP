namespace Tienda_Celulares_APP.Web.Services;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }

    public Guid MobilePhoneId { get; set; }
    public MobilePhone? MobilePhone { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
