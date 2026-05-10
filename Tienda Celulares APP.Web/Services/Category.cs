namespace Tienda_Celulares_APP.Web.Services;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<MobilePhone> MobilePhones { get; set; } = new();
}
