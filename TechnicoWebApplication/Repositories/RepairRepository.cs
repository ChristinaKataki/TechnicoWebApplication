using Microsoft.EntityFrameworkCore;
using TechnicoWebApplication.Context;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Repositories;
public class RepairRepository : IRepository<Repair, long>
{
    private readonly TechnicoDbContext _dbContext;

    public RepairRepository(TechnicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Repair> Create(Repair t)
    {
        _dbContext.Repairs.Add(t);
        await _dbContext.SaveChangesAsync();
        return t;
    }

    public async Task<bool> Delete(long id)
    {
        Repair? repair = _dbContext.Repairs.Find(id);

        if (repair == null)
        {
            return false;
        }

        _dbContext.Repairs.Remove(repair);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<Repair?> Read(long id)
    {
        return await _dbContext.Repairs
            .Include(repair => repair.PropertyOwner)
            .FirstOrDefaultAsync(repair => repair.Id == id);
    }

    public async Task<List<Repair>> FindByOwner(string vat)
    {
        return await _dbContext.Repairs
            .Where(repair => repair.PropertyOwner.Vat == vat)
            .ToListAsync();
    }

    public Task<List<Repair>> Read()
    {
        throw new NotImplementedException();
    }

    public async Task<Repair?> Update(long id, Repair repair)
    {
        var existingItem = await _dbContext.Repairs.FindAsync(id);

        if (existingItem == null)
        {
            return null;
        }

        _dbContext.Entry(existingItem).CurrentValues.SetValues(repair);

        await _dbContext.SaveChangesAsync();

        return existingItem;
    }
}
