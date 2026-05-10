using Microsoft.EntityFrameworkCore;

namespace Tienda_Celulares_APP.Web.Services;

public class InventoryService
{
    private readonly InventoryDbContext _db;

    public InventoryService(InventoryDbContext db)
    {
        _db = db;
    }

    public async Task<List<MobilePhone>> GetAllAsync() => await _db.MobilePhones.OrderBy(m => m.Marca).ThenBy(m => m.Modelo).ToListAsync();

    public async Task<MobilePhone?> GetAsync(Guid id) => await _db.MobilePhones.FindAsync(id);

    public async Task<MobilePhone> CreateAsync(MobilePhone m)
    {
        m.Id = Guid.NewGuid();
        _db.MobilePhones.Add(m);
        await _db.SaveChangesAsync();
        return m;
    }

    public async Task<bool> UpdateAsync(MobilePhone m)
    {
        var exist = await _db.MobilePhones.AnyAsync(x => x.Id == m.Id);
        if (!exist) return false;
        _db.MobilePhones.Update(m);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _db.MobilePhones.FindAsync(id);
        if (entity == null) return false;
        _db.MobilePhones.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    // Seed sample data
    public async Task SeedAsync()
    {
        if (!await _db.MobilePhones.AnyAsync())
        {
            _db.MobilePhones.Add(new MobilePhone { Id = Guid.NewGuid(), Marca = "XPhone", Modelo = "X1", Color = "Negro", Stock = 10, Precio = 499.99m });
            _db.MobilePhones.Add(new MobilePhone { Id = Guid.NewGuid(), Marca = "Alpha", Modelo = "A5", Color = "Blanco", Stock = 5, Precio = 299.99m });
            await _db.SaveChangesAsync();
        }
    }
}
