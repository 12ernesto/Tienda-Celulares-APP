namespace Tienda_Celulares_APP.Web.Services;

public class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;

    public List<MobilePhone> MobilePhones { get; set; } = new();
}
